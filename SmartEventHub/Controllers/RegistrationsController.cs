using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEventHub.Services;
using System.Security.Claims;

namespace SmartEventHub.Controllers
{
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationsController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        // ==========================================
        // 1. POST /api/sessions/{id}/register (Attendee)
        // Description: Register the current attendee for a specific session
        // ==========================================
        [HttpPost("/api/sessions/{id}/register")]
        [Authorize(Roles = "Attendee")]
        public async Task<IActionResult> Register(Guid id)
        {
            // Extract the authenticated user's ID from the JWT token
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;
            if (!Guid.TryParse(userIdString, out Guid attendeeId)) return Unauthorized();

            // Send request to the service layer
            var result = await _registrationService.RegisterForSessionAsync(id, attendeeId);

            // Handle business rule outcomes
            if (result == "SessionNotFound") return NotFound(new { message = "Session not found." });
            if (result == "SoldOut") return BadRequest(new { message = "Sorry, this session is fully booked." });
            if (result == "AlreadyRegistered") return BadRequest(new { message = "You are already registered for this session." });

            return Ok(new { message = "Successfully registered for the session." });
        }

        // ==========================================
        // 2. DELETE /api/registrations/{id} (Attendee)
        // Description: Cancel registration using Soft Delete
        // ==========================================
        [HttpDelete("/api/registrations/{id}")]
        [Authorize(Roles = "Attendee")]
        public async Task<IActionResult> CancelRegistration(Guid id)
        {
            // Extract the authenticated user's ID from the JWT token
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;
            if (!Guid.TryParse(userIdString, out Guid attendeeId)) return Unauthorized();

            // Execute soft delete in the service layer
            var success = await _registrationService.CancelRegistrationAsync(id, attendeeId);
            if (!success) return BadRequest(new { message = "Cancellation failed. Registration not found or already cancelled." });

            return Ok(new { message = "Registration cancelled successfully." });
        }

        // ==========================================
        // 3. GET /api/registrations/my (Attendee)
        // Description: List all registrations belonging to the current attendee
        // ==========================================
        [HttpGet("/api/registrations/my")]
        [Authorize(Roles = "Attendee")]
        public async Task<IActionResult> GetMyRegistrations()
        {
            // Extract the authenticated user's ID from the JWT token
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;
            if (!Guid.TryParse(userIdString, out Guid attendeeId)) return Unauthorized();

            // Fetch registrations for this user
            var registrations = await _registrationService.GetMyRegistrationsAsync(attendeeId);
            return Ok(registrations);
        }

        // ==========================================
        // 4. GET /api/sessions/{id}/attendees (Admin)
        // Description: List all active attendees registered for a specific session
        // ==========================================
        [HttpGet("/api/sessions/{id}/attendees")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSessionAttendees(Guid id)
        {
            // Fetch session attendees list
            var attendees = await _registrationService.GetSessionAttendeesAsync(id);
            return Ok(attendees);
        }
    }
}