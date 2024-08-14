using AuthService.Core.Models;

namespace AuthService.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        Task AddUser(User user);
        Task<IEnumerable<User>> GetUsersAsync();
    }
}
