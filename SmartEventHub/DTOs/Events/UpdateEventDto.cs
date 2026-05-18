using SmartEventHub.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartEventHub.DTOs.Events
{
    // DTO used when an Admin updates an existing Event
    public class UpdateEventDto
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

        // When updating, the Admin might want to change the status from Draft to Published
        [Required]
        public EventStatus Status { get; set; }
    }
}