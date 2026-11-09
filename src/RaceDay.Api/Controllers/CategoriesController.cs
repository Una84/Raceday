using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Core.Data;
using RaceDay.Core.Entities;

namespace RaceDay.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public CategoriesController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet("events/{eventId}/categories")]
        public async Task<IActionResult> GetCategoriesForEvent(int eventId)
        {
            if (!await _context.Events.AnyAsync(e => e.EventID == eventId))
            {
                return NotFound(new { message = "Event does not exist." });
            }

            var categories = await _context.Categories
                .Where(c => c.EventID == eventId)
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Event)
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null) return NotFound(new { message = "Category does not exist." });
            return Ok(category);
        }

        [HttpPost("events/{eventId}/categories")]
        public async Task<IActionResult> CreateCategory(int eventId, [FromBody] Category model)
        {
            if (!await _context.Events.AnyAsync(e => e.EventID == eventId))
            {
                return NotFound(new { message = "Event does not exist." });
            }

            model.EventID = eventId;
            _context.Categories.Add(model);
            await _context.SaveChangesAsync();

            return StatusCode(201, model);
        }

        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category model)
        {
            if (id != model.CategoryID) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(c => c.CategoryID == id)) return NotFound();
                throw;
            }

            return Ok(new { message = "Category updated successfully." });
        }

        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound(new { message = "Category does not exist." });

            category.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
