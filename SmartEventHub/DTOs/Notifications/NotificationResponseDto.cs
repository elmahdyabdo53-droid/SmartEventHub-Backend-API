namespace SmartEventHub.DTOs.Notifications
{
    public class NotificationResponseDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
    }
}