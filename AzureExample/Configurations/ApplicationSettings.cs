namespace AzureExample.Configurations
{
    public class ApplicationSettings
    {
        public static string SectionKey => "Application";

        public bool DebugMode { get; set; }
    }
}
