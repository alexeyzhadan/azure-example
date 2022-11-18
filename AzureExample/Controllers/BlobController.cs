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
        public async Task<IActionResult> GetMedia(CancellationToken cancellationToken)
        {
            var blobs = await blobService.GetMediaAsync(cancellationToken);

            return Ok(blobs);
        }

        [HttpPost]
        public async Task<IActionResult> UploadMedia([FromForm] IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null)
            { 
                return BadRequest();
            }

            var result = await blobService.UploadMediaAsync(file, cancellationToken);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }
    }
}