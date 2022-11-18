namespace AzureExample.Exceptions
{
    public class BlobException : Exception
    {
        private const string TITLE = "Azure Blob issue";

        public string Title { get; }

        public BlobException(string message)
            : base(message)
        {
            Title = TITLE;
        }

        public BlobException(string message, Exception innerException)
            : base(message, innerException)
        {
            Title = TITLE;
        }

        public BlobException(string title, string message)
            : base(message)
        {
            Title = title;
        }

        public BlobException(string title, string message, Exception innerException)
            : base(message, innerException)
        {
            Title = title;
        }
    }
}
