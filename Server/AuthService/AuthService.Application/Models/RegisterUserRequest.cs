namespace AuthService.Application.Models;
public class RegisterUserRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
}


public class LoginRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

