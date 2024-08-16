using AuthService.Application.Interfaces;
using AuthService.Application.Models;
using AuthService.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IUserAuthService service;

	public AuthController(IUserAuthService service)
	{
		this.service = service;
	}


    [HttpGet("/")]
    public IActionResult Welcome()
    {
        return Ok("Welcome to the Auth Service");
    }

    [HttpPost("register")]
	[Authorize(Roles ="Admin")]
	public async Task<IActionResult> Register([FromBody]RegisterUserRequest model)
	{
		await service.RegisterUser(model);
		return Ok();
	}

    [Authorize(Roles = "Admin")]
	[HttpGet("users")]
	public async Task<IEnumerable<User>> GetUsers()
	{
		return await service.GetUsersAsync();
	}

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var res = await service.Login(model);
        return Ok(res);
    }
}
