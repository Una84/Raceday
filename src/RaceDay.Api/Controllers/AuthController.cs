using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Core.Data;
using RaceDay.Core.Entities;
using System.Security.Cryptography;
using System.Text;

namespace RaceDay.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public AuthController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return Conflict(new { message = "Email already exists." });
            }

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            if (dto.Role == "Organiser")
            {
                var organiser = new Organiser
                {
                    UserID = user.UserID,
                    OrganisationName = dto.OrganisationName ?? $"{dto.FirstName}'s Organisation"
                };
                _context.Organisers.Add(organiser);
                await _context.SaveChangesAsync();
            }

            return StatusCode(201, new { message = "User account created successfully.", user.UserID });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || user.PasswordHash != HashPassword(dto.Password))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // In a session-based or token setup, return user info and role state
            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("UserRole", user.Role);

            return Ok(new { message = "Login successful.", user.UserID, user.Email, user.Role });
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    public record RegisterDto(string FirstName, string LastName, string Email, string Password, string? PhoneNumber, string Role, string? OrganisationName);
    public record LoginDto(string Email, string Password);
}
