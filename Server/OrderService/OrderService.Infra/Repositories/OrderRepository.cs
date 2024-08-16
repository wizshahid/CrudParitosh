using Microsoft.Azure.Cosmos;
using OrderService.Core.Interfaces;
using OrderService.Core.Models;
using OrderService.Infra.Data;

namespace OrderService.Infra.Repositories;
public class OrderRepository : IOrderRepository
{
    private readonly CosmosDbContext _context;

    public OrderRepository(CosmosDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        return await _context.OrdersContainer.CreateItemAsync(order, new PartitionKey(order.Id));
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        var query = _context.OrdersContainer.GetItemQueryIterator<Order>(
            new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
            .WithParameter("@userId", userId)
        );

        List<Order> results = new List<Order>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }
        return results;
    }


    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        var query = _context.OrdersContainer.GetItemQueryIterator<Order>();
        List<Order> results = new();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }
        return results;
    }

    public async Task<Order> GetOrderByIdAsync(string id)
    {
        try
        {
            return await _context.OrdersContainer.ReadItemAsync<Order>(id, new PartitionKey(id));
        }
        catch (CosmosException)
        {
            return null;
        }
    }

    public async Task UpdateOrderAsync(Order order)
    {
        await _context.OrdersContainer.ReplaceItemAsync(order, order.Id, new PartitionKey(order.Id));
    }
}

