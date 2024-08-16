using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderService.Application.Interfaces;
using OrderService.Core.Interfaces;
using OrderService.Core.Models;

namespace OrderService.Application.Services;
public class OrdersService : IOrdersService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ServiceBusClient _serviceBusClient;
    private readonly string orderCountUpdateQueueName;

    public OrdersService(IOrderRepository orderRepository, /*ServiceBusClient serviceBusClient,*/ IConfiguration configuration)
    {
        _orderRepository = orderRepository;
        //_serviceBusClient = serviceBusClient;
        orderCountUpdateQueueName = configuration["AzureServiceBus:OrderCountUpdateQueueName"]!;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        var createdOrder = await _orderRepository.CreateOrderAsync(order);

        //await SendMessageToUpdateOrderCountAsync(order.ProductId, order.Quantity);

        return createdOrder;
    }

    private async Task SendMessageToUpdateOrderCountAsync(string productId, int quantity)
    {
        var messagePayload = new
        {
            ProductId = productId,
            Quantity = quantity
        };

        var messageBody = JsonConvert.SerializeObject(messagePayload);
        var message = new ServiceBusMessage(messageBody);

        var sender = _serviceBusClient.CreateSender(orderCountUpdateQueueName);
        await sender.SendMessageAsync(message);
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await _orderRepository.GetOrdersByUserIdAsync(userId);
    }

    public async Task UpdateOrderProductNameAsync(string productId, string newProductName)
    {
        var orders = await _orderRepository.GetOrdersAsync();

        foreach (var order in orders.Where(o => o.ProductId == productId))
        {
            order.ProductName = newProductName;
            await _orderRepository.UpdateOrderAsync(order);
        }
    }
}

