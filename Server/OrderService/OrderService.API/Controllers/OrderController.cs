using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Interfaces;
using OrderService.Core.Models;

namespace OrderService.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrdersService _orderService;

    public OrderController(IOrdersService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("/")]
    public IActionResult Welcome()
    {
        return Ok("Welcome to the Order Service");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        var createdOrder = await _orderService.CreateOrderAsync(order);
        return Ok(createdOrder);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderByUserId(string id)
    {
        var order = await _orderService.GetOrdersByUserIdAsync(id);
        return Ok(order);
    }
}

