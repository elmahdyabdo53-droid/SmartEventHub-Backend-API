using SmartEventHub.DTOs.Events;
using SmartEventHub.Entities;
using SmartEventHub.Enums;
using SmartEventHub.Repositories;

namespace SmartEventHub.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ==========================================
        // 1. Get All Published Events (With Pagination)
        // ==========================================
        public async Task<IEnumerable<EventResponseDto>> GetAllPublishedAsync(int pageNumber, int pageSize)
        {
            // Fetch all events from the database
            var allEvents = await _unitOfWork.Events.GetAllAsync();

            // Filter by 'Published' status, then apply Pagination logic
            var publishedEvents = allEvents
                .Where(e => e.Status == EventStatus.Published)
                .OrderBy(e => e.StartDate) // Best practice: order events by date
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Map the Entities to DTOs
            var responseList = new List<EventResponseDto>();
            foreach (var e in publishedEvents)
            {
                // Fetch the organizer to get their name
                var organizer = await _unitOfWork.Users.GetByIdAsync(e.OrganizerId);

                responseList.Add(new EventResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Location = e.Location,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Status = e.Status.ToString(), // Converts Enum (2) to String ("Published")
                    OrganizerName = organizer?.FullName ?? "Unknown"
                });
            }

            return responseList;
        }

        // ==========================================
        // 2. Get Event By ID
        // ==========================================
        public async Task<EventResponseDto?> GetByIdAsync(Guid id)
        {
            var e = await _unitOfWork.Events.GetByIdAsync(id);
            if (e == null) return null;

            var organizer = await _unitOfWork.Users.GetByIdAsync(e.OrganizerId);

            return new EventResponseDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Location = e.Location,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Status = e.Status.ToString(),
                OrganizerName = organizer?.FullName ?? "Unknown"
            };
        }

        // ==========================================
        // 3. Create a New Event
        // ==========================================
        public async Task<EventResponseDto> CreateAsync(CreateEventDto dto, Guid organizerId)
        {
            // Map DTO to Entity
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                OrganizerId = organizerId, // Assigned automatically from the logged-in user's token
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = EventStatus.Draft // Business Logic: New events are always Drafts initially
            };

            await _unitOfWork.Events.AddAsync(newEvent);
            await _unitOfWork.CompleteAsync();

            var organizer = await _unitOfWork.Users.GetByIdAsync(organizerId);

            // Return the created event as a DTO
            return new EventResponseDto
            {
                Id = newEvent.Id,
                Title = newEvent.Title,
                Description = newEvent.Description,
                Location = newEvent.Location,
                StartDate = newEvent.StartDate,
                EndDate = newEvent.EndDate,
                Status = newEvent.Status.ToString(),
                OrganizerName = organizer?.FullName ?? "Unknown"
            };
        }

        // ==========================================
        // 4. Update Event
        // ==========================================
        public async Task<bool> UpdateAsync(Guid id, UpdateEventDto dto)
        {
            var existingEvent = await _unitOfWork.Events.GetByIdAsync(id);
            if (existingEvent == null) return false;

            // Update the fields
            existingEvent.Title = dto.Title;
            existingEvent.Description = dto.Description;
            existingEvent.Location = dto.Location;
            existingEvent.StartDate = dto.StartDate;
            existingEvent.EndDate = dto.EndDate;
            existingEvent.Status = dto.Status; // Admin can change Draft to Published here

            _unitOfWork.Events.Update(existingEvent);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        // ==========================================
        // 5. Cancel Event (Soft Delete)
        // ==========================================
        public async Task<bool> CancelAsync(Guid id)
        {
            var existingEvent = await _unitOfWork.Events.GetByIdAsync(id);
            if (existingEvent == null) return false;

            // Soft delete: We don't remove it from DB, we just change its status
            existingEvent.Status = EventStatus.Cancelled;

            _unitOfWork.Events.Update(existingEvent);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}