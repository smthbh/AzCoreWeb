using Microsoft.AspNetCore.Mvc;
using AzCoreWeb.Server.Models;
using AzCoreWeb.Models;

namespace AzCoreWeb.Server.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpPost(Name = "Create")]
        public IActionResult Create([FromBody] GameAccount account)
        {
            // Create account logic here
            return Ok();
        }
    }
}
