using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureExample.Configurations;
using AzureExample.Exceptions;
using AzureExample.Models;
using Microsoft.Extensions.Options;

namespace AzureExample.Services
{
    public class BlobService
    {
        private readonly BlobStorageSettings settings;
        private readonly BlobServiceClient blobServiceClient;

        public BlobService(IOptions<BlobStorageSettings> options)
        {
            settings = options.Value;
            blobServiceClient = new BlobServiceClient(settings.ConnectionString);
        }

        public async Task<List<BlobItem>> GetBlobsAsync(string blobContainerName, CancellationToken cancellationToken)
        {
            var blobContainer = await GetOrCreateBlobContainerAsync(blobContainerName, cancellationToken);

            return await blobContainer.GetBlobsAsync(cancellationToken: cancellationToken).ToListAsync();
        }

        public async Task<List<BlobItem>> GetMediaAsync(CancellationToken cancellationToken)
        {
            return await GetBlobsAsync(settings.MediaContainerName, cancellationToken);
        }

        public async Task<ResultModel> UploadBlobAsync(string blobContainerName, IFormFile file, CancellationToken cancellationToken)
        {
            var blobContainer = await GetOrCreateBlobContainerAsync(blobContainerName, cancellationToken);

            var blob = blobContainer.GetBlobClient(file.FileName);
            if (await blob.ExistsAsync(cancellationToken))
            {
                return new ResultModel($"File with name {file.FileName} already exists.");
            }

            using (var stream = file.OpenReadStream())
            { 
                var response = (await blob.UploadAsync(stream, cancellationToken))?
                    .GetRawResponse();
                if (response == null || response.IsError)
                {
                    throw new BlobException(response.ToString());
                }
            }

            return new ResultModel();
        }

        public async Task<ResultModel> UploadMediaAsync(IFormFile file, CancellationToken cancellationToken)
        {
            return await UploadBlobAsync(settings.MediaContainerName, file, cancellationToken);
        }

        private async Task<BlobContainerClient> GetOrCreateBlobContainerAsync(string blobContainerName, CancellationToken cancellationToken)
        {
            var blobContainer = new BlobContainerClient(settings.ConnectionString, blobContainerName);

            if (!await blobContainer.ExistsAsync(cancellationToken))
            {
                var response = (await blobServiceClient.CreateBlobContainerAsync(blobContainerName))?
                    .GetRawResponse();
                if (response == null || response.IsError)
                {
                    throw new BlobException(response.ToString());
                }
            }

            return blobContainer;
        }
    }
}
