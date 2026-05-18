namespace SmartEventHub.Enums
{
    public enum EventStatus
    {
        // Event is still being planned, not visible to attendees
        Draft = 1,

        // Event is live and attendees can view/register for sessions
        Published = 2,

        // Event has been cancelled by the organizer
        Cancelled = 3
    }
}
