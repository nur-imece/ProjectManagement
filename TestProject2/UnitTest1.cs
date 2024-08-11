using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectManagement.Api.Controllers;
using ProjectManagement.Business;
using ProjectManagement.Model.Request;
using ProjectManagement.Model.Respond;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectManagement.Model.Response;
using Xunit;


namespace TestProject2
{
    public class AccountControllerTests
    {
        private readonly AccountController _controller;
        private readonly Mock<IAccountService> _accountServiceMock;

        public AccountControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _controller = new AccountController(_accountServiceMock.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnOkResult_WhenRegisterIsSuccessful()
        {
            var registerRequest = new RegisterRequest { /* Geçerli kayıt bilgileri */ };
            _accountServiceMock.Setup(service => service.RegisterAsync(registerRequest))
                .ReturnsAsync(IdentityResult.Success); // IdentityResult döndürür

            var result = await _controller.Register(registerRequest) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        [Fact]
        public async Task Login_ShouldReturnOkResult_WhenLoginIsSuccessful()
        {
            var loginRequest = new LoginRequest { /* Geçerli giriş bilgileri */ };
            var authResponse = new AuthResponse
            {
                Token = "sample-jwt-token" // Örnek JWT token
            };

            _accountServiceMock.Setup(service => service.LoginAsync(loginRequest))
                .ReturnsAsync(authResponse); // AuthResponse döndürür

            var result = await _controller.Login(loginRequest) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            var response = result.Value as AuthResponse;
            Assert.NotNull(response);
            Assert.Equal(authResponse.Token, response.Token); // Dönen token'ın doğru olduğunu kontrol etme
        }



        // Diğer testler...
    }
}