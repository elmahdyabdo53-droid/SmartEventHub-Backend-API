using SmartEventHub.DTOs.Registrations;

namespace SmartEventHub.Services
{
    public interface IRegistrationService
    {
        // Returns a string to indicate the specific result (e.g., "Success", "SoldOut", "AlreadyRegistered")
        Task<string> RegisterForSessionAsync(Guid sessionId, Guid attendeeId);

        // Returns true if cancellation is successful, false otherwise
        Task<bool> CancelRegistrationAsync(Guid registrationId, Guid attendeeId);

        // Returns a list of registrations for a specific attendee
        Task<IEnumerable<RegistrationResponseDto>> GetMyRegistrationsAsync(Guid attendeeId);

        // Returns a list of attendees for a specific session (Admin only)
        Task<IEnumerable<AttendeeResponseDto>> GetSessionAttendeesAsync(Guid sessionId);
    }
}