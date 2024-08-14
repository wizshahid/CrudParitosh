using AuthService.Application.Interfaces;
using AuthService.Application.Models;
using AuthService.Core.Interfaces;
using AuthService.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Application.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly string? _jwtSecret;

        public UserAuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtSecret = configuration["Jwt:Secret"];
        }

        public async Task RegisterUser(RegisterUserRequest model)
        {
            var existingUser = await _userRepository.GetUserByUsername(model.Username);
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = model.Username,
                PasswordHash = hashedPassword,
                Role = model.Role,
            };

           await _userRepository.AddUser(user);
        }

        public async Task<string> Login(LoginRequest model)
        {
            var user = await _userRepository.GetUserByUsername(model.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password");
            }

            return GenerateJwtToken(user);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
