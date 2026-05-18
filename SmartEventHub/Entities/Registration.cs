namespace SmartEventHub.Entities
{
    // Tracks which attendee registered for which session
    public class Registration
    {
        public Guid Id { get; set; }

        // Foreign Key to the Attendee (User)
        public Guid UserId { get; set; }

        // Foreign Key to the Session
        public Guid SessionId { get; set; }

        public DateTime RegisteredAt { get; set; }

        // Soft delete flag (If true, it means the user cancelled their registration)
        public bool IsCancelled { get; set; } = false;

        // ==========================================
        // Navigation Properties
        // ==========================================

        public User User { get; set; }

        public Session Session { get; set; }
    }
}