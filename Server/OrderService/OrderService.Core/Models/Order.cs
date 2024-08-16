using Newtonsoft.Json;

namespace OrderService.Core.Models;
public class Order
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ProductId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public int Price { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
}

