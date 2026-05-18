using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEventHub.Services;
using System.Security.Claims;

namespace SmartEventHub.Controllers
{
    [ApiController]
    [Authorize] // Requires authentication via Bearer token for all endpoints
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // ==========================================
        // 1. GET /api/notifications
        // Description: Get current user's notification inbox
        // ==========================================
        [HttpGet("/api/notifications")]
        public async Task<IActionResult> GetInbox()
        {
            // Extract user ID from token
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId)) return Unauthorized();

            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        // ==========================================
        // 2. PUT /api/notifications/{id}/read
        // Description: Mark a single notification as read
        // ==========================================
        [HttpPut("/api/notifications/{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId)) return Unauthorized();

            var success = await _notificationService.MarkAsReadAsync(id, userId);
            if (!success) return BadRequest(new { message = "Notification not found, already read, or unauthorized." });

            return NoContent(); // 204 No Content
        }

        // ==========================================
        // 3. PUT /api/notifications/read-all
        // Description: Mark all notifications as read
        // ==========================================
        [HttpPut("/api/notifications/read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId)) return Unauthorized();

            var success = await _notificationService.MarkAllAsReadAsync(userId);
            if (!success) return BadRequest(new { message = "No unread notifications found." });

            return NoContent(); // 204 No Content
        }
    }
}