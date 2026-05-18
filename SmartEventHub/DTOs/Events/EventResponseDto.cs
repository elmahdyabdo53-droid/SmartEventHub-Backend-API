namespace SmartEventHub.DTOs.Events
{
    // DTO used to send Event details to the client (hides database complexities)
    public class EventResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // We return the status as a readable string (e.g., "Published") instead of a number like 2
        public string Status { get; set; }

        // Instead of returning the OrganizerId (Guid), it's more user-friendly to return the Organizer's Name
        public string OrganizerName { get; set; }
    }
}