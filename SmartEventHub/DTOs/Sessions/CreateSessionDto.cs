using System.ComponentModel.DataAnnotations;

namespace SmartEventHub.DTOs.Sessions
{
    // DTO used when an Admin creates a new Session for an Event
    public class CreateSessionDto
    {
        [Required]
        public Guid EventId { get; set; }

        [Required]
        public Guid SpeakerId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Room { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Capacity must be between 1 and 1000")]
        public int Capacity { get; set; }

        // Note: We don't ask for RegisteredCount here because a new session always starts with 0 attendees.
    }
}