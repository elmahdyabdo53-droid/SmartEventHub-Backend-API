namespace SmartEventHub.DTOs.Sessions
{
    // DTO used to return Session details to the user
    public class SessionResponseDto
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        public string Title { get; set; }

        public string Room { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Capacity { get; set; }

        // Tells the user how many people have already registered
        public int RegisteredCount { get; set; }

        // We return the speaker's name instead of their Guid ID for better UX
        public string SpeakerName { get; set; }
    }
}