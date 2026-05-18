using SmartEventHub.DTOs.Sessions;
using SmartEventHub.Entities;
using SmartEventHub.Repositories;

namespace SmartEventHub.Services
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ==========================================
        // 1. Create a New Session
        // ==========================================
        public async Task<SessionResponseDto?> CreateSessionAsync(CreateSessionDto dto)
        {
            // Business Rule 1: Start time must be before End time
            if (dto.StartTime >= dto.EndTime)
            {
                // We return null here. In a real massive project, we might throw a custom exception.
                return null;
            }

            // Business Rule 2: The Event must exist
            var eventExists = await _unitOfWork.Events.GetByIdAsync(dto.EventId);
            if (eventExists == null)
            {
                return null;
            }

            // Business Rule 3: The Speaker must exist
            var speakerExists = await _unitOfWork.Users.GetByIdAsync(dto.SpeakerId);
            if (speakerExists == null)
            {
                return null;
            }

            // Map DTO to Entity
            var newSession = new Session
            {
                Id = Guid.NewGuid(),
                EventId = dto.EventId,
                SpeakerId = dto.SpeakerId,
                Title = dto.Title,
                Room = dto.Room,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Capacity = dto.Capacity,
                RegisteredCount = 0 // Starts empty
            };

            // Save to database
            await _unitOfWork.Sessions.AddAsync(newSession);
            await _unitOfWork.CompleteAsync();

            // Return the created session as a Response DTO
            return new SessionResponseDto
            {
                Id = newSession.Id,
                EventId = newSession.EventId,
                Title = newSession.Title,
                Room = newSession.Room,
                StartTime = newSession.StartTime,
                EndTime = newSession.EndTime,
                Capacity = newSession.Capacity,
                RegisteredCount = newSession.RegisteredCount,
                SpeakerName = speakerExists.FullName // We return the string name, not the Guid
            };
        }

        // ==========================================
        // 2. Get Sessions By Event ID
        // ==========================================
        public async Task<IEnumerable<SessionResponseDto>> GetSessionsByEventIdAsync(Guid eventId)
        {
            // Find all sessions where EventId matches the requested event
            var sessions = await _unitOfWork.Sessions.FindAsync(s => s.EventId == eventId);

            var responseList = new List<SessionResponseDto>();

            // Loop through each session to fetch the Speaker's name
            foreach (var session in sessions)
            {
                var speaker = await _unitOfWork.Users.GetByIdAsync(session.SpeakerId);

                responseList.Add(new SessionResponseDto
                {
                    Id = session.Id,
                    EventId = session.EventId,
                    Title = session.Title,
                    Room = session.Room,
                    StartTime = session.StartTime,
                    EndTime = session.EndTime,
                    Capacity = session.Capacity,
                    RegisteredCount = session.RegisteredCount,
                    SpeakerName = speaker?.FullName ?? "Unknown Speaker"
                });
            }

            // Order sessions by StartTime so they appear chronologically
            return responseList.OrderBy(s => s.StartTime);
        }


        // ==========================================
        // 3. Get Sessions By Speaker ID
        // ==========================================
        public async Task<IEnumerable<SessionResponseDto>> GetSessionsBySpeakerIdAsync(Guid speakerId)
        {
            var sessions = await _unitOfWork.Sessions.FindAsync(s => s.SpeakerId == speakerId);

            var responseList = new List<SessionResponseDto>();
            foreach (var session in sessions)
            {
                var speaker = await _unitOfWork.Users.GetByIdAsync(session.SpeakerId);
                responseList.Add(new SessionResponseDto
                {
                    Id = session.Id,
                    EventId = session.EventId,
                    Title = session.Title,
                    Room = session.Room,
                    StartTime = session.StartTime,
                    EndTime = session.EndTime,
                    Capacity = session.Capacity,
                    RegisteredCount = session.RegisteredCount,
                    SpeakerName = speaker?.FullName ?? "Unknown"
                });
            }
            return responseList.OrderBy(s => s.StartTime);
        }

        // ==========================================
        // 4. Get Session By ID
        // ==========================================
        public async Task<SessionResponseDto?> GetByIdAsync(Guid id)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(id);
            if (session == null) return null;

            var speaker = await _unitOfWork.Users.GetByIdAsync(session.SpeakerId);
            return new SessionResponseDto
            {
                Id = session.Id,
                EventId = session.EventId,
                Title = session.Title,
                Room = session.Room,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Capacity = session.Capacity,
                RegisteredCount = session.RegisteredCount,
                SpeakerName = speaker?.FullName ?? "Unknown Speaker"
            };
        }

        // ==========================================
        // 5. Update Session (PUT)
        // ==========================================
        public async Task<bool> UpdateAsync(Guid id, UpdateSessionDto dto)
        {
            if (dto.StartTime >= dto.EndTime) return false;

            var existingSession = await _unitOfWork.Sessions.GetByIdAsync(id);
            if (existingSession == null) return false;

            // Senior Trick: Cannot reduce capacity below the number of currently registered attendees
            if (dto.Capacity < existingSession.RegisteredCount) return false;

            existingSession.Title = dto.Title;
            existingSession.Room = dto.Room;
            existingSession.StartTime = dto.StartTime;
            existingSession.EndTime = dto.EndTime;
            existingSession.Capacity = dto.Capacity;

            _unitOfWork.Sessions.Update(existingSession);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // ==========================================
        // 6. Delete Session (DELETE)
        // ==========================================
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingSession = await _unitOfWork.Sessions.GetByIdAsync(id);
            if (existingSession == null) return false;

            _unitOfWork.Sessions.Remove(existingSession);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}