using Microsoft.IdentityModel.Tokens;
using SmartEventHub.DTOs.Auth;
using SmartEventHub.Entities;
using SmartEventHub.Enums;
using SmartEventHub.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartEventHub.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        // ==========================================
        // 1. Registration Logic
        // ==========================================
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto model)
        {
            var existingUsers = await _unitOfWork.Users.FindAsync(u => u.Email == model.Email);
            if (existingUsers.Any())
            {
                return new AuthResponseDto { IsSuccess = false, Message = "Email is already registered!" };
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = model.FullName,
                Email = model.Email,
                Role = UserRole.Attendee,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "User registered successfully.",
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        // ==========================================
        // 2. Login Logic
        // ==========================================
        public async Task<AuthResponseDto> LoginAsync(LoginDto model)
        {
            var users = await _unitOfWork.Users.FindAsync(u => u.Email == model.Email);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                return new AuthResponseDto { IsSuccess = false, Message = "Invalid email or password." };
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return new AuthResponseDto { IsSuccess = false, Message = "Invalid email or password." };
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Login successful.",
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        // ==========================================
        // 3. JWT Token Generator Helper
        // ==========================================
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var durationInDays = Convert.ToDouble(_configuration["Jwt:DurationInDays"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(durationInDays),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}