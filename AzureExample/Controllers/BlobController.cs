using AzureExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureExample.Controllers
{
    public class BlobController : BaseApiController
    {
        private readonly BlobService blobService;

        public BlobController(BlobService blobService)
        {
            this.blobService = blobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlobsAsync(CancellationToken cancellationToken)
        {
            var blobs = await blobService.GetBlobsAsync(cancellationToken);

            return Ok(blobs);
        }

        [HttpPost]
        public async Task<IActionResult> UploadMediaFileAsync(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null)
            { 
                return BadRequest();
            }

            var blob = await blobService.UploadMediaAsync(file, cancellationToken);

            return Ok(blob);
        }
    }
}