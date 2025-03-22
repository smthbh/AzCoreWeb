using Microsoft.AspNetCore.Mvc;
using AzCoreWeb.Server.Models;
using AzCoreWeb.Models;
using AzCoreWeb.Server.Models.Response;
using System.Text.RegularExpressions;

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
            // Validate input
            if (string.IsNullOrWhiteSpace(account.Username) || string.IsNullOrWhiteSpace(account.Password))
            {
                return BadRequest("Username and password cannot be empty.");
            }

            if (account.Username.Length < 3 || account.Password.Length < 4)
            {
                return BadRequest("Username must be at least 3 characters and password at least 4 characters long.");
            }

            // Check for special characters in username
            if (Regex.IsMatch(account.Username, @"[^a-zA-Z0-9]"))
            {
                return BadRequest("Username cannot contain special characters.");
            }

            // Check for unsafe special characters in password
            if (Regex.IsMatch(account.Password, @"[^a-zA-Z0-9!@#$%^&*()_+=-]"))
            {
                return BadRequest("Password contains unsafe special characters.");
            }

            // Sanitize input
            // remove leading/trailing spaces
            var sanitizedUsername = account.Username.Trim();
            var sanitizedPassword = account.Password.Trim();

            try
            {
                var response = await _accounts.CreateUser(sanitizedUsername, sanitizedPassword);
                return Ok(new StatusResponse { Info = response });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
