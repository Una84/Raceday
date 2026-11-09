using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Core.Data;
using RaceDay.Core.Entities;

namespace RaceDay.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpPost("enrolments/{enrolmentId}/results")]
        public async Task<IActionResult> CaptureResult(int enrolmentId, [FromBody] Result model)
        {
            if (!await _context.Enrolments.AnyAsync(e => e.EnrolmentID == enrolmentId))
            {
                return NotFound(new { message = "Enrolment does not exist." });
            }

            if (await _context.Results.AnyAsync(r => r.EnrolmentID == enrolmentId))
            {
                return Conflict(new { message = "Result already exists for this enrolment." });
            }

            model.EnrolmentID = enrolmentId;
            _context.Results.Add(model);
            await _context.SaveChangesAsync();

            return StatusCode(201, model);
        }

        [HttpGet("results/my")]
        public async Task<IActionResult> GetMyResults()
        {
            var userId = HttpContext.Session.GetInt32("UserID") ?? 1;

            var results = await _context.Results
                .Include(r => r.Enrolment)
                .ThenInclude(e => e.Event)
                .Include(r => r.Enrolment)
                .ThenInclude(e => e.Category)
                .Where(r => r.Enrolment.UserID == userId)
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet("results/{id}")]
        public async Task<IActionResult> GetResult(int id)
        {
            var result = await _context.Results
                .Include(r => r.Enrolment)
                .ThenInclude(e => e.Event)
                .Include(r => r.Enrolment)
                .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (result == null) return NotFound(new { message = "Result does not exist." });
            return Ok(result);
        }

        [HttpPut("results/{id}")]
        public async Task<IActionResult> UpdateResult(int id, [FromBody] Result model)
        {
            if (id != model.ResultID) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Results.Any(r => r.ResultID == id)) return NotFound();
                throw;
            }

            return Ok(new { message = "Result updated successfully." });
        }

        [HttpDelete("results/{id}")]
        public async Task<IActionResult> DeleteResult(int id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result == null) return NotFound(new { message = "Result does not exist." });

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
