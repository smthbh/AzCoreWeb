using AzCoreWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AzCoreWeb.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ServerInfo _serverInfo;

        public StatusController(ServerInfo status)
        {
            _serverInfo = status;
        }

        [HttpGet(Name = "Info")]
        public async Task<IActionResult> Info()
        {
            try
            {
                var info = await _serverInfo.GetInfo();
                return Ok(info);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
