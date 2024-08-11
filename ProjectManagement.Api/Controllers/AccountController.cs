using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Model.Request;
using ProjectManagement.Model.Response;
using ProjectManagement.Business;
using System.Threading.Tasks;


namespace ProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]  // Bu endpoint için kimlik doğrulama gerekmiyor
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.RegisterAsync(request);

            if (result.Succeeded)
                return Ok(new { Message = "User registered successfully" });

            return BadRequest(result.Errors);
        }

        [AllowAnonymous]  // Bu endpoint için kimlik doğrulama gerekmiyor
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _accountService.LoginAsync(request);

            if (response != null)
                return Ok(response);

            return Unauthorized(new { Message = "Invalid username or password" });
        }

        [Authorize]  // Bu endpoint için token doğrulaması zorunlu
        [HttpGet("protected")]
        public IActionResult ProtectedEndpoint()
        {
            return Ok(new { Message = "This is a protected endpoint, accessible only with a valid token." });
        }
    }
}