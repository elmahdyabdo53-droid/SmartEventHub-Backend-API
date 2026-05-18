using System.ComponentModel.DataAnnotations;

namespace SmartEventHub.DTOs.Events
{
    // DTO used when an Admin creates a new Event
    public class CreateEventDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        // Notice: We don't ask for OrganizerId here. We will get it from the JWT token automatically.
        // Notice: We don't ask for Status here. A new event is always created as a 'Draft' by default.
    }
}