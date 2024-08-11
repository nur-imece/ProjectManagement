using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectManagement.Model.Request;
using ProjectManagement.Model.Response;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace ProjectManagement.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<MongoIdentityUser<Guid>> _userManager;
        private readonly SignInManager<MongoIdentityUser<Guid>> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountService(UserManager<MongoIdentityUser<Guid>> userManager, SignInManager<MongoIdentityUser<Guid>> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterRequest request)
        {
            var user = new MongoIdentityUser<Guid> { UserName = request.Username, Email = request.Email };
            return await _userManager.CreateAsync(user, request.Password);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // JWT token oluşturma kodu buraya eklenecek
                var token = GenerateJwtToken(request.Email);
                return new AuthResponse { Token = token };
            }
            return null; // veya uygun bir hata mesajı dönebilirsiniz
        }

        private string GenerateJwtToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email)
                }),
                
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
