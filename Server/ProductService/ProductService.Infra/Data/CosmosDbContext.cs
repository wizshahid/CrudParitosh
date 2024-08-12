using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace ProductService.Infrastructure.Data
{
    public class CosmosDbContext
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Database _database;
        public Container ProductContainer { get; }

        public CosmosDbContext(IConfiguration configuration)
        {
            _cosmosClient = new CosmosClient(configuration["CosmosDb:AccountEndpoint"], configuration["CosmosDb:AccountKey"]);
            _database = _cosmosClient.CreateDatabaseIfNotExistsAsync(configuration["CosmosDb:DatabaseName"]).Result;

            ProductContainer = _database.CreateContainerIfNotExistsAsync("Products", "/id").Result;
        }
    }
}
