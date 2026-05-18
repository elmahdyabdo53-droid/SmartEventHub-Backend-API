using SmartEventHub.DTOs.Notifications;
using SmartEventHub.Entities;
using SmartEventHub.Repositories;

namespace SmartEventHub.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ==========================================
        // 1. Get current user's notification inbox
        // ==========================================
        public async Task<IEnumerable<NotificationResponseDto>> GetUserNotificationsAsync(Guid userId)
        {
            // Fetch all notifications belonging to the user
            var notifications = await _unitOfWork.Notifications.FindAsync(n => n.UserId == userId);
            var responseList = new List<NotificationResponseDto>();

            foreach (var note in notifications)
            {
                responseList.Add(new NotificationResponseDto
                {
                    Id = note.Id,
                    Message = note.Message,
                    Type = note.Type,
                    IsRead = note.IsRead,
                    SentAt = note.SentAt
                });
            }

            // Order by the most recent notifications first
            return responseList.OrderByDescending(n => n.SentAt);
        }

        // ==========================================
        // 2. Mark a notification as read
        // ==========================================
        public async Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);

            // Validate: Check if it exists, belongs to the user, and is currently unread
            if (notification == null || notification.UserId != userId || notification.IsRead)
                return false;

            // Mark as read and update the database
            notification.IsRead = true;
            _unitOfWork.Notifications.Update(notification);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        // ==========================================
        // 3. Mark all notifications as read
        // ==========================================
        public async Task<bool> MarkAllAsReadAsync(Guid userId)
        {
            // Fetch only unread notifications for this user
            var unreadNotifications = await _unitOfWork.Notifications.FindAsync(n => n.UserId == userId && !n.IsRead);

            // If there are no unread notifications, return false
            if (!unreadNotifications.Any()) return false;

            // Mark all fetched notifications as read
            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                _unitOfWork.Notifications.Update(notification);
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}