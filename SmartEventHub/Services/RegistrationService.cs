using SmartEventHub.DTOs.Registrations;
using SmartEventHub.Entities;
using SmartEventHub.Repositories;

namespace SmartEventHub.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ==========================================
        // 1. Register for a session
        // ==========================================
        public async Task<string> RegisterForSessionAsync(Guid sessionId, Guid attendeeId)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId);
            if (session == null) return "SessionNotFound";

            if (session.RegisteredCount >= session.Capacity) return "SoldOut";

            // Updated to use UserId
            var existingRegistrations = await _unitOfWork.Registrations.FindAsync(r => r.SessionId == sessionId && r.UserId == attendeeId);

            var alreadyRegistered = existingRegistrations.FirstOrDefault(r => !r.IsCancelled);
            if (alreadyRegistered != null) return "AlreadyRegistered";

            var registration = new Registration
            {
                Id = Guid.NewGuid(),
                SessionId = sessionId,
                UserId = attendeeId, // Updated to UserId
                RegisteredAt = DateTime.UtcNow, // Updated to RegisteredAt
                IsCancelled = false
            };

            await _unitOfWork.Registrations.AddAsync(registration);

            session.RegisteredCount += 1;
            _unitOfWork.Sessions.Update(session);

            await _unitOfWork.CompleteAsync();

            return "Success";
        }

        // ==========================================
        // 2. Cancel registration (Soft Delete)
        // ==========================================
        public async Task<bool> CancelRegistrationAsync(Guid registrationId, Guid attendeeId)
        {
            var registration = await _unitOfWork.Registrations.GetByIdAsync(registrationId);

            // Updated to use UserId
            if (registration == null || registration.UserId != attendeeId || registration.IsCancelled)
                return false;

            registration.IsCancelled = true;
            _unitOfWork.Registrations.Update(registration);

            var session = await _unitOfWork.Sessions.GetByIdAsync(registration.SessionId);
            if (session != null && session.RegisteredCount > 0)
            {
                session.RegisteredCount -= 1;
                _unitOfWork.Sessions.Update(session);
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        // ==========================================
        // 3. List own registrations (For Attendee)
        // ==========================================
        public async Task<IEnumerable<RegistrationResponseDto>> GetMyRegistrationsAsync(Guid attendeeId)
        {
            // Updated to use UserId
            var registrations = await _unitOfWork.Registrations.FindAsync(r => r.UserId == attendeeId);
            var responseList = new List<RegistrationResponseDto>();

            foreach (var reg in registrations)
            {
                var session = await _unitOfWork.Sessions.GetByIdAsync(reg.SessionId);
                responseList.Add(new RegistrationResponseDto
                {
                    Id = reg.Id,
                    SessionId = reg.SessionId,
                    SessionTitle = session?.Title ?? "Unknown Session",
                    RegistrationDate = reg.RegisteredAt, // Updated to map from RegisteredAt
                    IsCancelled = reg.IsCancelled
                });
            }

            return responseList.OrderByDescending(r => r.RegistrationDate);
        }

        // ==========================================
        // 4. List attendees for a session (Admin Only)
        // ==========================================
        public async Task<IEnumerable<AttendeeResponseDto>> GetSessionAttendeesAsync(Guid sessionId)
        {
            var registrations = await _unitOfWork.Registrations.FindAsync(r => r.SessionId == sessionId && !r.IsCancelled);
            var responseList = new List<AttendeeResponseDto>();

            foreach (var reg in registrations)
            {
                // Updated to use UserId
                var user = await _unitOfWork.Users.GetByIdAsync(reg.UserId);
                if (user != null)
                {
                    responseList.Add(new AttendeeResponseDto
                    {
                        AttendeeId = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        RegistrationDate = reg.RegisteredAt // Updated to map from RegisteredAt
                    });
                }
            }

            return responseList;
        }
    }
}