using System.Threading.Tasks;
using ProjectManagement.Model.Request;
using ProjectManagement.Model.Response;
using Microsoft.AspNetCore.Identity;


namespace ProjectManagement.Business
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}