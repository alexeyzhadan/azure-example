namespace AzureExample.Exceptions
{
    public class BlobException : Exception
    {
        public string Title { get; }

        public BlobException()
            : base()
        {
        }

        public BlobException(string message)
            : base(message)
        {
        }

        public BlobException(string message, Exception innerException)
            : base(message, innerException)
        {
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
