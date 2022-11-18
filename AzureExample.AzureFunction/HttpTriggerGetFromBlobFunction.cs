using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using System.Threading;

namespace AzureExample.AzureFunction
{
    public static class HttpTriggerGetFromBlobFunction
    {
        [FunctionName(nameof(HttpTriggerGetFromBlobFunction))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "HttpTriggerGetFromBlobFunction/{fileName}")] HttpRequest request,
            [Blob("media", Connection = "AzureWebJobsStorage")] BlobContainerClient mediaContainer,
            ILogger logger,
            string fileName,
            CancellationToken cancellationToken)
        {
            logger.LogInformation($"{nameof(HttpTriggerGetFromBlobFunction)}. C# HTTP trigger function processed a request.");

            if (await mediaContainer.ExistsAsync(cancellationToken))
            {
                var blobClient = mediaContainer.GetBlobClient(fileName);
                if (await blobClient.ExistsAsync(cancellationToken))
                {
                    var blobResponse = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);

                    return new OkObjectResult(blobResponse.Value.Content);
                }
            }

            return new BadRequestObjectResult($"File with name {fileName} is not found!");
        }
    }
}
