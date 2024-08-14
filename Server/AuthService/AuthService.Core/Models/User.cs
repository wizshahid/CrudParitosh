using Newtonsoft.Json;

namespace AuthService.Core.Models;
public class User
{
    [JsonProperty("id")]
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = null!;
}

