using System.Reflection;

// ReSharper disable CheckNamespace
namespace NServiceBus
    // ReSharper restore CheckNamespace
{
    public static class EndpointConfigurationExtensions
    {
        public static EndpointConfiguration UseLicence(
            this EndpointConfiguration endpointConfiguration,
            Assembly assembly)
        {
            endpointConfiguration.LicensePath(assembly.Location.Replace(assembly.GetName().Name + ".dll", "License.xml"));

            return endpointConfiguration;
        }
    }
}