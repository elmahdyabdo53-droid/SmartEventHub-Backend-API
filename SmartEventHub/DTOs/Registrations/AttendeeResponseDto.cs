namespace SmartEventHub.DTOs.Registrations
{
    public class AttendeeResponseDto
    {
        public Guid AttendeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}