using System.ComponentModel.DataAnnotations;

namespace SmartEventHub.DTOs.Auth
{
    // DTO used when a new user registers an account
    public class RegisterDto
    {
        // [Required] ensures the user cannot send a blank field
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress] // Automatically validates that the string is a proper email format
        public string Email { get; set; }

        [Required]
        [MinLength(6)] // Forces the password to be at least 6 characters for security
        public string Password { get; set; }

        // We do NOT ask for Role here. Attendees get the Attendee role by default.
        // We do NOT ask for Id or CreatedAt. The server handles those.
    }
}