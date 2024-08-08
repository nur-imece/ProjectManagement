using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Model;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AspNetCore.Identity.MongoDbCore.Models;

namespace ProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<MongoIdentityUser<Guid>> _userManager;
        private readonly SignInManager<MongoIdentityUser<Guid>> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<MongoIdentityUser<Guid>> userManager, SignInManager<MongoIdentityUser<Guid>> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new MongoIdentityUser<Guid> { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                return Ok(new { Message = "User registered successfully" });

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Burada kullanıcıyı doğrulama işlemini yapmanız gerekiyor.
            // Örnek olarak, her zaman başarılı olan bir doğrulama yapılıyor.
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, model.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }
    }
}
