
using Newtonsoft.Json;

namespace ProductService.Core.Models;
public class Product
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
    public int StockQuantity { get; set; }
    public string Manufacturer { get; set; } = null!;
}
