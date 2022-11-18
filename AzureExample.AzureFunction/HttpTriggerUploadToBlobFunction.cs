using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using System.Threading;
using System;

namespace AzureExample.AzureFunction
{
    public static class HttpTriggerUploadToBlobFunction
    {
        [FunctionName(nameof(HttpTriggerUploadToBlobFunction))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "HttpTriggerUploadToBlobFunction")] HttpRequest request,
            [Blob("media", Connection = "AzureWebJobsStorage")] BlobContainerClient mediaContainer,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            logger.LogInformation($"{nameof(HttpTriggerUploadToBlobFunction)}. C# HTTP trigger function processed a request.");

            try
            {
                var file = await GetFileFromRequestAsync(request, cancellationToken);
                if (file == null)
                {
                    return new BadRequestObjectResult("File to upload is not found!");
                }

                if (!(await mediaContainer.ExistsAsync(cancellationToken)))
                {
                    return new BadRequestObjectResult("Media container is not found!");
                }

                var blobClient = mediaContainer.GetBlobClient(file.FileName);
                if (await blobClient.ExistsAsync(cancellationToken))
                {
                    return new BadRequestObjectResult($"File with name {file.FileName} already exists!");
                }

                using (var stream = file.OpenReadStream())
                {
                    var response = (await mediaContainer.UploadBlobAsync(file.FileName, stream, cancellationToken))
                        .GetRawResponse();

                    return response.IsError
                        ? new BadRequestObjectResult(response.ReasonPhrase)
                        : new OkObjectResult(response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        private static async Task<IFormFile> GetFileFromRequestAsync(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            var formData = await httpRequest.ReadFormAsync(cancellationToken);

            return formData.Files.GetFile("file");
        }
    }
}
