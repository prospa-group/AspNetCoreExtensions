using Prospa.Extensions.NServiceBus.Behaviours;

// ReSharper disable CheckNamespace
namespace NServiceBus.Pipeline
    // ReSharper restore CheckNamespace
{
    public static class PipelineSettingsExtensions
    {
        public static PipelineSettings StripAssemblyVersionFromEnclosedMessageTypePipeline(this PipelineSettings pipeline)
        {
            pipeline.Register(
                behavior: new StripAssemblyNameFromEnclosedMessageTypeOutgoingHeaderBehavior(),
                description: "Strips assembly version from enclosed message type");

            return pipeline;
        }
    }
}