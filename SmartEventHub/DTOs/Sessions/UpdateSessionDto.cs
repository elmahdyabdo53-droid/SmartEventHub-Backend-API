using System.ComponentModel.DataAnnotations;

namespace SmartEventHub.DTOs.Sessions
{
    public class UpdateSessionDto
    {
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
        [Range(1, 1000)]
        public int Capacity { get; set; }
    }
}