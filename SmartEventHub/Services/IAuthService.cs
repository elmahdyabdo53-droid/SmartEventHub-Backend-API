using SmartEventHub.DTOs.Auth;

namespace SmartEventHub.Services
{
    // Contract for our Authentication Service
    public interface IAuthService
    {
        // Handles user registration logic
        Task<AuthResponseDto> RegisterAsync(RegisterDto model);

        // Handles user login logic
        Task<AuthResponseDto> LoginAsync(LoginDto model);
    }
}