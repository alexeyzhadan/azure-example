using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureExample.Configurations;
using Microsoft.Extensions.Options;

namespace AzureExample.Services
{
    public class BlobService
    {
        private readonly BlobContainerClient containerClient;

        public BlobService(IOptions<MediaBlobStorageSettings> options)
        {
            containerClient = new BlobContainerClient(options.Value.ConnectionString, options.Value.ContainerName);
        }

        public async Task<List<BlobItem>> GetBlobsAsync(CancellationToken cancellationToken)
        {
            return await containerClient.GetBlobsAsync(cancellationToken: cancellationToken).ToListAsync();
        }

        public async Task<BlobContentInfo> UploadMediaAsync(IFormFile file, CancellationToken cancellationToken)
        {
            using var stream = file.OpenReadStream();
            
            var response = await containerClient.UploadBlobAsync(file.FileName, stream, cancellationToken);

            return response.Value;
        }
    }
}
