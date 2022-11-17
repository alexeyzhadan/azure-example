using Microsoft.AspNetCore.Mvc;

namespace AzureExample.Controllers
{
    public class MainController : BaseApiController
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("Hello, World!!!");
        }
    }
}
