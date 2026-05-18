using SmartEventHub.DTOs.Sessions;

namespace SmartEventHub.Services
{
    // Contract for Session business logic
    public interface ISessionService
    {
        // Creates a new session after validating the rules
        Task<SessionResponseDto?> CreateSessionAsync(CreateSessionDto dto);

        // Gets all sessions belonging to a specific event
        Task<IEnumerable<SessionResponseDto>> GetSessionsByEventIdAsync(Guid eventId);


        // Gets all sessions for a specific speaker
        Task<IEnumerable<SessionResponseDto>> GetSessionsBySpeakerIdAsync(Guid speakerId);
        // Gets a single session by its ID
        Task<SessionResponseDto?> GetByIdAsync(Guid id);

        // 2 update and delete methods
        Task<bool> UpdateAsync(Guid id, UpdateSessionDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}