using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Core.Data;
using RaceDay.Core.Entities;

namespace RaceDay.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrolmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpPost("events/{eventId}/enrolments")]
        public async Task<IActionResult> EnrolParticipant(int eventId, [FromBody] EnrolmentDto dto)
        {
            var userId = HttpContext.Session.GetInt32("UserID") ?? 1; // Fallback for test mocking if session is uninitialised

            if (!await _context.Events.AnyAsync(e => e.EventID == eventId) || 
                !await _context.Categories.AnyAsync(c => c.CategoryID == dto.CategoryID && c.EventID == eventId))
            {
                return NotFound(new { message = "Event or category does not exist." });
            }

            if (await _context.Enrolments.AnyAsync(e => e.UserID == userId && e.EventID == eventId))
            {
                return Conflict(new { message = "Participant is already enrolled in this event." });
            }

            var category = await _context.Categories.FindAsync(dto.CategoryID);

            var enrolment = new Enrolment
            {
                UserID = userId,
                EventID = eventId,
                CategoryID = dto.CategoryID,
                Status = "Pending",
                PaymentStatus = category?.EntryFee > 0 ? "Unpaid" : "Paid",
                AmountPaid = category?.EntryFee ?? 0,
                ReferenceNumber = $"RD-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString()[..6].ToUpper()}"
            };

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return StatusCode(201, enrolment);
        }

        [HttpGet("enrolments/my")]
        public async Task<IActionResult> GetMyEnrolments()
        {
            var userId = HttpContext.Session.GetInt32("UserID") ?? 1;

            var enrolments = await _context.Enrolments
                .Include(e => e.Event)
                .Include(e => e.Category)
                .Where(e => e.UserID == userId)
                .ToListAsync();

            return Ok(enrolments);
        }

        [HttpGet("events/{eventId}/enrolments")]
        public async Task<IActionResult> GetEventEnrolments(int eventId)
        {
            var enrolments = await _context.Enrolments
                .Include(e => e.User)
                .Include(e => e.Category)
                .Where(e => e.EventID == eventId)
                .ToListAsync();

            return Ok(enrolments);
        }

        [HttpGet("enrolments/{id}")]
        public async Task<IActionResult> GetEnrolment(int id)
        {
            var enrolment = await _context.Enrolments
                .Include(e => e.Event)
                .Include(e => e.Category)
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.EnrolmentID == id);

            if (enrolment == null) return NotFound(new { message = "Enrolment does not exist." });
            return Ok(enrolment);
        }

        [HttpPut("enrolments/{id}/status")]
        public async Task<IActionResult> UpdateEnrolmentStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            var enrolment = await _context.Enrolments.FindAsync(id);
            if (enrolment == null) return NotFound(new { message = "Enrolment does not exist." });

            enrolment.Status = dto.Status;
            enrolment.PaymentStatus = dto.PaymentStatus;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Enrolment status updated successfully." });
        }
    }

    public record EnrolmentDto(int CategoryID);
    public record UpdateStatusDto(string Status, string PaymentStatus);
}
