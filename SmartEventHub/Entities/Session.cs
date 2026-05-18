namespace SmartEventHub.Entities
{
    // Represents an individual session within an Event
    public class Session
    {
        public Guid Id { get; set; }

        // Foreign Key to the Event table
        public Guid EventId { get; set; }

        // Foreign Key to the User table (The Speaker)
        public Guid SpeakerId { get; set; }

        public string Title { get; set; }

        // Optional speaker-submitted abstract
        public string? Abstract { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        // Maximum number of attendees allowed
        public int Capacity { get; set; }

        // Tracks how many people have registered (Default is 0)
        public int RegisteredCount { get; set; } = 0;

        public string Room { get; set; }

        // ==========================================
        // Navigation Properties
        // ==========================================

        // The Event this session belongs to
        public Event Event { get; set; }

        // The Speaker assigned to this session
        public User Speaker { get; set; }

        // One Session can have many Registrations
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}