using Microsoft.AspNetCore.Mvc;
using SmartEventHub.DTOs.Auth;
using SmartEventHub.Services;

namespace SmartEventHub.Controllers
{
    // Sets the base URL for this controller (e.g., /api/auth)
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Dependency Injection: We need the AuthService (The Kitchen) to process our requests
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ==========================================
        // 1. POST: /api/auth/register [cite: 347]
        // ==========================================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            // Step 1: Check if the incoming data meets our DTO requirements ([Required], [EmailAddress], etc.)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns 400 Bad Request with validation errors
            }

            // Step 2: Send the valid data to the service layer to handle business logic
            var result = await _authService.RegisterAsync(model);

            // Step 3: Return the appropriate HTTP response based on the service's result
            if (!result.IsSuccess)
            {
                return BadRequest(result); // Returns 400 Bad Request (e.g., Email already exists)
            }

            return Ok(result); // Returns 200 OK along with the JWT Token and user info
        }

        // ==========================================
        // 2. POST: /api/auth/login [cite: 347]
        // ==========================================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // Step 1: Validate incoming data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Step 2: Let the service handle password verification and token generation
            var result = await _authService.LoginAsync(model);

            // Step 3: Return response
            if (!result.IsSuccess)
            {
                return Unauthorized(result); // Returns 401 Unauthorized (Invalid credentials)
            }

            return Ok(result); // Returns 200 OK with the generated JWT Token
        }
    }
}