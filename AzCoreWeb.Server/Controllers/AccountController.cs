using Microsoft.AspNetCore.Mvc;
using AzCoreWeb.Server.Models;
using AzCoreWeb.Models;
using AzCoreWeb.Server.Models.Response;

namespace AzCoreWeb.Server.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly Accounts _accounts;

        public AccountController(Accounts accounts)
        {
            _accounts = accounts;
        }

        [HttpPost(Name = "Create")]
        public async Task<IActionResult> Create([FromBody] GameAccount account)
        {
            // Create account logic here
            try
            {
                var response = await _accounts.CreateUser(account.Username, account.Password);
                return Ok(new StatusResponse { Info = response });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
