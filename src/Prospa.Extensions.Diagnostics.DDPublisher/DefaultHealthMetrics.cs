namespace Prospa.Extensions.Diagnostics.DDPublisher
{
    public static class DefaultHealthMetrics
    {
        public const string AzureStorageAccountConnectivity = "metric_name:p3.service_health_azure_storage_account_connectivity";
        public const string AzureStorageContainerConnectivity = "metric_name:p3.service_health_azure_storage_container_connectivity";
        public const string AzureCosmosDbConnectivity = "metric_name:p3.service_health_azure_cosmos_connectivity";
        public const string AzureCosmosDbCollectionConnectivity = "metric_name:p3.service_health_azure_cosmos_collection_connectivity";
        public const string AzureSqlConnectivity = "metric_name:p3.service_health_azure_sql_connectivity";

        public static string AzureResourceName(string name)
        {
            return $"azure_resource_name:{name}";
        }

        public static string AzureEntityName(string name)
        {
            return $"azure_entity_name:{name}";
        }
    }
}