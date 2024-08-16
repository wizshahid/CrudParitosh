using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace OrderService.Infra.Data;
public class CosmosDbContext
{
    private readonly CosmosClient _cosmosClient;
    private readonly Database _database;
    public Container OrdersContainer { get; }

    public CosmosDbContext(IConfiguration configuration)
    {
        _cosmosClient = new CosmosClient(configuration["CosmosDb:AccountEndpoint"], configuration["CosmosDb:AccountKey"]);
        _database = _cosmosClient.CreateDatabaseIfNotExistsAsync(configuration["CosmosDb:DatabaseName"]).Result;

        OrdersContainer = _database.CreateContainerIfNotExistsAsync("Orders", "/id").Result;
    }
}
