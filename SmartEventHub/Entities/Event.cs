using SmartEventHub.Enums;
using static System.Collections.Specialized.BitVector32;


namespace SmartEventHub.Entities
{
    // Represents an Event created by an Admin
    public class Event
    {
        // Primary Key
        public Guid Id { get; set; }

        // Foreign Key linking to the User (Organizer/Admin)
        public Guid OrganizerId { get; set; }

        public string Title { get; set; }

        // Nullable string (?) because description is optional
        public string? Description { get; set; }

        public string Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        // Event status using our custom Enum (Draft, Published, Cancelled)
        public EventStatus Status { get; set; }

        // ==========================================
        // Navigation Properties
        // ==========================================

        // The Organizer of this event
        public User Organizer { get; set; }

        // One Event can have many Sessions
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
