using AuthService.Core.Interfaces;
using AuthService.Infra.Data;
using Microsoft.Azure.Cosmos;
using User = AuthService.Core.Models.User;

namespace AuthService.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CosmosDbContext _context;

        public UserRepository(CosmosDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Username = @username")
                          .WithParameter("@username", username);

            var iterator = _context.UserContainer.GetItemQueryIterator<User>(query);
            var result = await iterator.ReadNextAsync();
            return result?.FirstOrDefault();
        }

        public async Task AddUser(User user)
        {
            user.Id = Guid.NewGuid().ToString();  
            await _context.UserContainer.CreateItemAsync(user, new PartitionKey(user.Id));
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var query = _context.UserContainer.GetItemQueryIterator<User>();
            List<User> results = new();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
    }
}
