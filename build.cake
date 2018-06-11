#tool "nuget:https://www.nuget.org/api/v2?package=xunit.runner.console"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target                  = Argument("target", "Default");
var configuration           = HasArgument("BuildConfiguration") ? Argument<string>("BuildConfiguration") : EnvironmentVariable("BuildConfiguration") != null ? EnvironmentVariable("BuildConfiguration") : "Release";								  
var buildNumber             = HasArgument("BuildNumber") ? Argument<int>("BuildNumber") : TFBuild.IsRunningOnVSTS ? int.Parse(TFBuild.Environment.Build.Number) : 0;
var branch                  = TFBuild.IsRunningOnVSTS && EnvironmentVariable("BUILD_SOURCEBRANCH") != null ? EnvironmentVariable("BUILD_SOURCEBRANCH") : "dev"; // TFBuild.Environment.Repository.Branch doesn't provide the full branch name with a forward-slash
var isBranchForRelease      = branch.Contains("rel/"); // release branch convention is rel/{version};
var versionSuffix           = !isBranchForRelease ? "alpha" : XmlPeek("version.props", "/Project/PropertyGroup/VersionSuffix/text()");
var linkSources				= HasArgument("LinkSources") ? Argument<bool>("LinkSources") : EnvironmentVariable("LinkSources") != null ? bool.Parse(EnvironmentVariable("LinkSources")) : true;

//////////////////////////////////////////////////////////////////////
// DEFINE FILES & DIRECTORIES
//////////////////////////////////////////////////////////////////////

var packDirs				= new [] {  Directory("./src/Prospa.Extensions.AspNetCore.Authorization/Prospa.Extensions.AspNetCore.Authorization.csproj"), 
                                        Directory("./src/Prospa.Extensions.AspNetCore.Http/Prospa.Extensions.AspNetCore.Http.csproj"),
										Directory("./src/Prospa.Extensions.AspNetCore.Mvc.Core/Prospa.Extensions.AspNetCore.Mvc.Core.csproj"),
										Directory("./src/Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger/Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.csproj"),
										Directory("./src/Prospa.Extensions.AspNetCore.Serilog/Prospa.Extensions.AspNetCore.Serilog.csproj"),
										Directory("./src/Prospa.Extensions.AspNetCore.Swagger/Prospa.Extensions.AspNetCore.Swagger.csproj"),
										Directory("./src/Prospa.Extensions.Serilog/Prospa.Extensions.Serilog.csproj")
									};
var publishDirs				= new string[0];
var testPatterns			= new [] { "**/*.Facts/*.csproj", "**/*.IntegrationTests/*.csproj" };
var artifactsDir			= (DirectoryPath) Directory("./.artifacts");
var workingDir				= (DirectoryPath) Directory("./.working");
var testResultsDir			= (DirectoryPath) artifactsDir.Combine("test-results");
var publishedAppsDir		= (DirectoryPath) workingDir.Combine("published-apps");
var packagesDir				= artifactsDir.Combine("packages");
var packagesStableDir		= artifactsDir.Combine("stable");
var solutionFile			= "./AspNetCore.sln";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
	Context.Information("Deleting all .trx files");
	var rootDir = new System.IO.DirectoryInfo("./");
	rootDir.GetFiles("*.trx", SearchOption.AllDirectories).ToList().ForEach(file=>file.Delete());

	var settings = new DeleteDirectorySettings { Recursive = true, Force = true };

	if (DirectoryExists(artifactsDir))
	{
		DeleteDirectory(artifactsDir, settings); 
	}

	if (DirectoryExists(workingDir))
	{
		DeleteDirectory(workingDir, settings); 
	}

	if (DirectoryExists(testResultsDir))
	{
		DeleteDirectory(testResultsDir, settings); 
	}
});

Task("Init")
	.IsDependentOn("Clean")
    .Does(() =>
{
	Context.Information("Apps to Publish:");

	foreach(var directory in publishDirs)
	{
		Information("	Directory: {0}", directory);
	}

	Context.Information("Test Pattern: {0}", testPatterns);
	Context.Information("Artifacts Directory: {0}", artifactsDir);
	Context.Information("Working Directory: {0}", workingDir);
	Context.Information("Test Results Directory: {0}", testResultsDir);
	Context.Information("Published Applications Directory: {0}", publishedAppsDir);
	Context.Information("Packages Directory: {0}", packagesDir);
	Context.Information("Solution File: {0}", solutionFile);

    Context.Information("Version suffix: {0}", versionSuffix);
});

Task("Restore")
    .IsDependentOn("Init")
    .Does(() =>
{
	var settings = new NuGetRestoreSettings()
	{
		ConfigFile = "./NuGet.config", 
	};

	NuGetRestore(solutionFile, settings);
});

Task("Build")
    .Does(() =>
{	
	var settings = new DotNetCoreBuildSettings
	{
		Configuration = configuration,
		ArgumentCustomization = args => 
		{
			if (linkSources) 
			{
				args.Append("/p:SourceLinkCreate=true");
			}	

			args.Append($"--no-restore");

			return args;
		}
	};

	DotNetCoreBuild(solutionFile, settings);
});

Task("Pack")
	.IsDependentOn("Init")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        Configuration = configuration,
        OutputDirectory = packagesDir.FullPath,
		NoBuild = true,
		ArgumentCustomization = args => 
		{
			args.Append($"--no-restore");

			return args;
		}
    };	

	if (isBranchForRelease)
	{		
		if (!string.IsNullOrWhiteSpace(versionSuffix))
		{
			 settings.VersionSuffix = "rel-" + versionSuffix + "-" + buildNumber.ToString("D4");
		}
		else if (string.IsNullOrWhiteSpace(versionSuffix))
		{
			 settings.VersionSuffix = "rel-" + buildNumber.ToString("D4");
		}
	}	
	else
	{
		 settings.VersionSuffix = versionSuffix + "-" + buildNumber.ToString("D4");
	}

	Context.Information("Packing pre-release artifacts. Version suffix: " + settings.VersionSuffix);

    foreach(var packDir in packDirs)
    {
		Context.Information("Packaging " + packDir);
        DotNetCorePack(packDir, settings);
    }    
});

// When on a release branch (rel/{version}) re-pack without building to drop the build number from the package version).
// This is to allow publishing packages without having version gabs between stable releases (by dropping the build number)
// and avoiding having to rebuild the source.
Task("PackRelease")
    .WithCriteria(() => isBranchForRelease)
    .IsDependentOn("Pack")
    .Does(() =>
{
    var releaseSettings = new DotNetCorePackSettings
    {		
        Configuration = configuration,
        OutputDirectory = packagesStableDir.FullPath,
        NoBuild = true,
		ArgumentCustomization = args => 
		{
			args.Append($"--no-restore");

			return args;
		}
    };

    if (!string.IsNullOrEmpty(versionSuffix))
    {
        releaseSettings.VersionSuffix = versionSuffix;            
    }

	Context.Information("Packing release artifacts. Version suffix: " + releaseSettings.VersionSuffix);

	foreach(var packDir in packDirs)
    {
		Context.Information("Packaging " + packDir);
        DotNetCorePack(packDir, releaseSettings);
    }    
});

Task("Publish")
    .Does(() =>
{
	foreach(var publishDir in publishDirs)
    {
		var folderName = publishDir.ToString().Split('/').Last();

		var settings = new DotNetCorePublishSettings
		{
			Configuration = configuration,
			OutputDirectory = publishedAppsDir.FullPath + "/" + folderName,
			ArgumentCustomization = args => 
			{
				args.Append($"--no-restore");

				return args;
			}
		};	

		Context.Information("Publishing {0}", publishDir);
        DotNetCorePublish(publishDir, settings);

		var zipRootDirectory = settings.OutputDirectory;
		var zipTargetFile = artifactsDir.FullPath.ToString() + "/" + folderName + ".zip";
		var zipFiles = settings.OutputDirectory + "/*.*";

		if (!DirectoryExists(artifactsDir.FullPath.ToString()))
		{
			CreateDirectory(artifactsDir.FullPath.ToString());
		}

		var files = GetFiles(zipFiles);
		Zip(zipRootDirectory, zipTargetFile, files);
    }    
});

Task("RunTests")
    .IsDependentOn("Build")	
    .Does(() =>
{
    var projects = new FilePathCollection(PathComparer.Default); 
	
	foreach(var testPattern in testPatterns)
	{
		var files = GetFiles(testPattern);
		projects.Add(files);
	}

    Context.Information("Found {0} projects", projects.Count());

    foreach (var project in projects)
    {
		var settings = new DotNetCoreTestSettings()
		{
			Configuration = configuration,
			NoBuild = true,
			ArgumentCustomization = args => 
			{
				args.Append("--no-restore --logger:trx");

				return args;
			}
		};

		DotNetCoreTest(project.FullPath, settings);
    }
});


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

 Task("Default")
    .IsDependentOn("RunTests")
    .IsDependentOn("PackRelease");

Task("Local")
	.IsDependentOn("Restore")
    .IsDependentOn("RunTests")
    .IsDependentOn("PackRelease");	

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);