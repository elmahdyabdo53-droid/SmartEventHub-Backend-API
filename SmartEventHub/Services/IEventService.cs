using SmartEventHub.DTOs.Events;

namespace SmartEventHub.Services
{
    // Contract for Event business logic
    public interface IEventService
    {
        // Get all published events with pagination
        Task<IEnumerable<EventResponseDto>> GetAllPublishedAsync(int pageNumber, int pageSize);

        // Get a specific event by its ID
        Task<EventResponseDto?> GetByIdAsync(Guid id);

        // Create a new event (Needs the DTO + the Organizer's ID extracted from the token)
        Task<EventResponseDto> CreateAsync(CreateEventDto dto, Guid organizerId);

        // Update an existing event
        Task<bool> UpdateAsync(Guid id, UpdateEventDto dto);

        // Cancel an event (Soft delete)
        Task<bool> CancelAsync(Guid id);
    }
}