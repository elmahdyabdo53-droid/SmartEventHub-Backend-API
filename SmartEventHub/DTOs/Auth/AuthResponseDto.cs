namespace SmartEventHub.DTOs.Auth
{
    // DTO used to send data back to the client after a successful Login or Register
    public class AuthResponseDto
    {
        // The JWT Token that the client will use for future requests
        public string Token { get; set; }

        // Success or failure message
        public string Message { get; set; }

        // True if the auth process was successful
        public bool IsSuccess { get; set; }

        // Basic user info to display on the frontend
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}