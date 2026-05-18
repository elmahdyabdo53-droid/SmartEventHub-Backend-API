namespace SmartEventHub.Entities
{
    // Persisted notification records for the user inbox
    public class Notification
    {
        public Guid Id { get; set; }

        // Foreign Key to the Recipient (User)
        public Guid UserId { get; set; }

        public string Message { get; set; }

        // Type of notification (e.g., SessionFull, RoomChanged, Cancelled)
        public string Type { get; set; }

        // Indicates if the user has read the notification
        public bool IsRead { get; set; } = false;

        public DateTime SentAt { get; set; }

        // ==========================================
        // Navigation Property
        // ==========================================

        public User User { get; set; }
    }
}