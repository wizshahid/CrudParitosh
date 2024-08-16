using OrderService.Core.Models;

namespace OrderService.Application.Interfaces;
public interface IOrdersService
{
    Task<Order> CreateOrderAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    Task UpdateOrderProductNameAsync(string productId, string newProductName);
}
