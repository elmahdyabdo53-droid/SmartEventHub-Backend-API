using System.ComponentModel.DataAnnotations;

namespace SmartEventHub.DTOs.Auth
{
    // DTO used when a user attempts to log in
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}