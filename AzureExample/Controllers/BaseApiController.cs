using Microsoft.AspNetCore.Mvc;

namespace AzureExample.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
