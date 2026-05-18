using SmartEventHub.Enums;
using static System.Collections.Specialized.BitVector32;


namespace SmartEventHub.Entities
{
    // Represents the Users table in the database
    public class User
    {
        // Primary Key: Unique identifier for the user (using Guid for security)
        public Guid Id { get; set; }

        // Full name of the user (Maximum 100 characters)
        public string FullName { get; set; }

        // Email address used for login (Must be unique in database configuration)
        public string Email { get; set; }

        // Hashed password for security (Never store plain text passwords)
        public string PasswordHash { get; set; }

        // The role of the user (Admin, Speaker, or Attendee)
        public UserRole Role { get; set; }

        // Timestamp when the user account was created (UTC time)
        public DateTime CreatedAt { get; set; }




        // ==========================================
        // Navigation Properties (Database Relationships)
        // ==========================================


        // One Admin can organize many Events
        public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();

        // One Speaker can have many Sessions
        public ICollection<Session> SpeakerSessions { get; set; } = new List<Session>();

        // One Attendee can have many Registrations
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        // One User can receive many Notifications
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    
    }
}
