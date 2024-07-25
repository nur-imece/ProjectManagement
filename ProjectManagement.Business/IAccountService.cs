//using ProjectManagement.Data.Entities;
using System.Threading.Tasks;

namespace ProjectManagement.Model
{
    public interface IAccountService
    {
        Task<User> Authenticate(string username, string password);
        Task Register(User user);
    }
}