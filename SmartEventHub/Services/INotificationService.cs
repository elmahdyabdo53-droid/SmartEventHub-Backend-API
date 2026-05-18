using SmartEventHub.DTOs.Notifications;

namespace SmartEventHub.Services
{
    public interface INotificationService
    {
        // Retrieves the notification inbox for the current user
        Task<IEnumerable<NotificationResponseDto>> GetUserNotificationsAsync(Guid userId);

        // Marks a specific notification as read
        Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId);

        // Marks all unread notifications as read for the current user
        Task<bool> MarkAllAsReadAsync(Guid userId);
    }
}