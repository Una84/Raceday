using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Core.Data;
using RaceDay.Core.Entities;
using Xunit;

namespace RaceDay.Tests.Controllers
{
    // Controller implementation pattern matching your Part 2 API setup for Enrolments
    [Route("api/events/{eventId}/enrolments")]
    [ApiController]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrolmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId, [FromBody] EnrolmentDto dto)
        {
            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null) return NotFound(new { message = "Event does not exist." });

            var category = await _context.Categories.FindAsync(dto.CategoryID);
            if (category == null || category.EventID != eventId) return BadRequest(new { message = "Invalid category." });

            var exists = await _context.Enrolments.AnyAsync(e => e.UserID == dto.UserID && e.EventID == eventId);
            if (exists) return Conflict(new { message = "Participant is already enrolled." });

            var enrolment = new Enrolment
            {
                UserID = dto.UserID,
                EventID = eventId,
                CategoryID = dto.CategoryID,
                Status = "Confirmed",
                PaymentStatus = "Paid",
                AmountPaid = category.EntryFee,
                ReferenceNumber = $"RD-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
            };

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return StatusCode(201, enrolment);
        }
    }

    public record EnrolmentDto(int UserID, int CategoryID);

    public class EnrolmentsControllerTests
    {
        private RaceDayDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new RaceDayDbContext(options);
        }

        [Fact]
        public async Task Enrol_ValidParticipantAndCategory_ReturnsCreated()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            
            var user = new User { UserID = 3, FirstName = "Sipho", LastName = "Dlamini", Email = "sipho@example.com", PasswordHash = "hash", Role = "Participant" };
            var org = new Organiser { OrganiserID = 1, UserID = 1, OrganisationName = "Limpopo Runners" };
            var ev = new Event { EventID = 1, OrganiserID = 1, EventName = "Polokwane City Run", EventDate = DateTime.Today, Location = "Polokwane", StartTime = new TimeSpan(6,30,0), EventType = "Run" };
            var category = new Category { CategoryID = 1, EventID = 1, CategoryName = "21.1 km Open", EntryFee = 180.00m };

            context.Users.Add(user);
            context.Organisers.Add(org);
            context.Events.Add(ev);
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var controller = new EnrolmentsController(context);
            var dto = new EnrolmentDto(UserID: 3, CategoryID: 1);

            // Act
            var result = await controller.Enrol(1, dto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
            Assert.Single(context.Enrolments);
        }

        [Fact]
        public async Task Enrol_DuplicateEnrolment_ReturnsConflict()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            
            context.Enrolments.Add(new Enrolment
            {
                UserID = 3,
                EventID = 1,
                CategoryID = 1,
                Status = "Confirmed"
            });
            await context.SaveChangesAsync();

            var controller = new EnrolmentsController(context);
            var dto = new EnrolmentDto(UserID: 3, CategoryID: 1);

            // Act
            var result = await controller.Enrol(1, dto);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
        }
    }
}
