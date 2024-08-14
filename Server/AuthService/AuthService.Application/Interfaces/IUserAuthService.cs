using AuthService.Application.Models;
using AuthService.Core.Models;

namespace AuthService.Application.Interfaces;
public interface IUserAuthService
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<string> Login(LoginRequest model);
    Task RegisterUser(RegisterUserRequest model);
}
