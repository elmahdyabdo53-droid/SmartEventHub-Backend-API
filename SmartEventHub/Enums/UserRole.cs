namespace SmartEventHub.Enums
{
    public enum UserRole
    {
        // System Administrator (Has full access to create events)
        Admin = 1,

        // Speaker (Assigned to sessions)
        Speaker = 2,

        // Regular attendee (Registers for sessions)
        Attendee = 3
    }
}
