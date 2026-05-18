namespace SmartEventHub.DTOs.Registrations
{
    public class RegistrationResponseDto
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public string SessionTitle { get; set; } 
        public DateTime RegistrationDate { get; set; }
        public bool IsCancelled { get; set; }
    }
}