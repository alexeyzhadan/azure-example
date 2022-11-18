namespace AzureExample.Configurations
{
    public class BlobStorageSettings
    {
        public static string SectionKey => "StorageAccount:BlobStorage";

        public string ConnectionString { get; set; }
        public string MediaContainerName { get; set; }
    }
}
