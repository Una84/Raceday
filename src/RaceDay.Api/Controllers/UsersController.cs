using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Core.Data;
using RaceDay.Core.Entities;

namespace RaceDay.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public UsersController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return Unauthorized(new { message = "User is not logged in." });

            var user = await _context.Users
                .Include(u => u.OrganiserProfile)
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null) return NotFound(new { message = "User profile not found." });

            return Ok(new
            {
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PhoneNumber,
                user.Role,
                user.CreatedAt,
                user.OrganiserProfile
            });
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return Unauthorized(new { message = "User is not logged in." });

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound(new { message = "User profile not found." });

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully." });
        }
    }

    public record UpdateProfileDto(string FirstName, string LastName, string? PhoneNumber);
}
