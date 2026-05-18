using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEventHub.DTOs.Sessions;
using SmartEventHub.Services;
using System.Security.Claims;

namespace SmartEventHub.Controllers
{
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        // 1. GET /api/events/{eventId}/sessions (Public)
        [HttpGet("/api/events/{eventId}/sessions")]
        public async Task<IActionResult> GetEventSessions(Guid eventId)
        {
            var sessions = await _sessionService.GetSessionsByEventIdAsync(eventId);
            return Ok(sessions);
        }

        // 2. GET /api/sessions/{id} (Public)
        [HttpGet("/api/sessions/{id}")]
        public async Task<IActionResult> GetSessionDetail(Guid id)
        {
            var session = await _sessionService.GetByIdAsync(id);
            if (session == null) return NotFound(new { message = "Session not found." });
            return Ok(session);
        }

        // 3. POST /api/events/{eventId}/sessions (Admin)
        [HttpPost("/api/events/{eventId}/sessions")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSession(Guid eventId, [FromBody] CreateSessionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            dto.EventId = eventId;
            var createdSession = await _sessionService.CreateSessionAsync(dto);
            if (createdSession == null) return BadRequest(new { message = "Creation failed." });
            return Ok(createdSession);
        }

        // 4. PUT /api/sessions/{id} (Admin)
        [HttpPut("/api/sessions/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSession(Guid id, [FromBody] UpdateSessionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var success = await _sessionService.UpdateAsync(id, dto);
            if (!success) return BadRequest(new { message = "Update failed." });
            return NoContent();
        }

        // 5. DELETE /api/sessions/{id} (Admin)
        [HttpDelete("/api/sessions/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveSession(Guid id)
        {
            var success = await _sessionService.DeleteAsync(id);
            if (!success) return NotFound(new { message = "Session not found." });
            return Ok(new { message = "Session removed." });
        }

        // 6. GET /api/speakers/sessions (Speaker)
        [HttpGet("/api/speakers/sessions")]
        [Authorize(Roles = "Speaker")]
        public async Task<IActionResult> GetOwnSessions()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;
            if (!Guid.TryParse(userIdString, out Guid speakerId)) return Unauthorized();

            var sessions = await _sessionService.GetSessionsBySpeakerIdAsync(speakerId);
            return Ok(sessions);
        }
    }
}