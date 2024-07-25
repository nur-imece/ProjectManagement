using Microsoft.AspNetCore.Mvc;
//using ProjectManagement.Business.Services;
//using ProjectManagement.Data.Entities;
using System.Threading.Tasks;
using ProjectManagement.Model;

namespace ProjectManagement.API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            var user = await _accountService.Authenticate(loginUser.UserName, loginUser.Password);
            if (user == null)
                return Unauthorized("Invalid username or password");

            // Generate token (JWT, etc.) and return
            // var token = GenerateJwtToken(user);
            // return Ok(new { token });

            return Ok(user);  // Simplified for now
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User newUser)
        {
            await _accountService.Register(newUser);
            return Ok("User registered successfully");
        }
    }
}

