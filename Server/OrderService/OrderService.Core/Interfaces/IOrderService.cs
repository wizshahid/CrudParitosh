using OrderService.Core.Models;

namespace OrderService.Core.Interfaces;
public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order> GetOrderByIdAsync(string id);
    Task UpdateOrderAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
}
