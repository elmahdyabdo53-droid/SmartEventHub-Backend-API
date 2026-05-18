using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEventHub.DTOs.Events;
using SmartEventHub.Services;
using System.Security.Claims;

namespace SmartEventHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // ==========================================
        // 1. GET: /api/events?pageNumber=1&pageSize=10
        // Description: Public endpoint to get all published events
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> GetAllPublished([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var events = await _eventService.GetAllPublishedAsync(pageNumber, pageSize);
            return Ok(events);
        }

        // ==========================================
        // 2. GET: /api/events/{id}
        // Description: Public endpoint to get event details
        // ==========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var eventDto = await _eventService.GetByIdAsync(id);
            if (eventDto == null) return NotFound(new { message = "Event not found" });

            return Ok(eventDto);
        }

        // ==========================================
        // 3. POST: /api/events
        // Description: Admin only endpoint to create a new event
        // ==========================================
        [HttpPost]
        [Authorize(Roles = "Admin")] // ONLY users with Admin role in their token can access this
        public async Task<IActionResult> Create([FromBody] CreateEventDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Extract the Organizer's ID directly from their JWT Token
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdString, out Guid organizerId))
            {
                return Unauthorized(new { message = "Invalid token claims" });
            }

            var createdEvent = await _eventService.CreateAsync(dto, organizerId);

            // Returns 201 Created and points to the GetById endpoint to view the new event
            return CreatedAtAction(nameof(GetById), new { id = createdEvent.Id }, createdEvent);
        }

        // ==========================================
        // 4. PUT: /api/events/{id}
        // Description: Admin only endpoint to update an event (e.g. Draft -> Published)
        // ==========================================
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEventDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _eventService.UpdateAsync(id, dto);
            if (!success) return NotFound(new { message = "Event not found" });

            return NoContent(); // Returns 204 No Content indicating success
        }

        // ==========================================
        // 5. DELETE: /api/events/{id}
        // Description: Admin only endpoint to cancel an event (Soft Delete)
        // ==========================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var success = await _eventService.CancelAsync(id);
            if (!success) return NotFound(new { message = "Event not found" });

            return Ok(new { message = "Event has been cancelled successfully" });
        }
    }
}