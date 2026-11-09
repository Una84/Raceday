using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Controllers;
using RaceDay.Core.Data;
using RaceDay.Core.Entities;
using Xunit;

namespace RaceDay.Tests.Controllers
{
    public class EventsControllerTests
    {
        private RaceDayDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new RaceDayDbContext(options);
        }

        [Fact]
        public async Task GetEvents_ReturnsActiveEvents()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var organiser = new Organiser { OrganiserID = 1, UserID = 1, OrganisationName = "Limpopo Runners" };
            context.Organisers.Add(organiser);
            context.Events.Add(new Event
            {
                EventID = 1,
                OrganiserID = 1,
                EventName = "Polokwane City Run",
                EventDate = DateTime.Today.AddDays(10),
                Location = "Polokwane",
                StartTime = new TimeSpan(6, 30, 0),
                EventType = "Run",
                IsActive = true
            });
            await context.SaveChangesAsync();

            var controller = new EventsController(context);

            // Act
            var result = await controller.GetEvents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var events = Assert.IsType<List<Event>>(okResult.Value);
            Assert.Single(events);
        }

        [Fact]
        public async Task GetEvent_InvalidId_ReturnsNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new EventsController(context);

            // Act
            var result = await controller.GetEvent(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateEvent_ValidModel_ReturnsCreatedResult()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new EventsController(context);
            var newEvent = new Event
            {
                OrganiserID = 1,
                EventName = "Cape Town Cycle Challenge",
                EventDate = DateTime.Today.AddDays(20),
                Location = "Cape Town",
                StartTime = new TimeSpan(6, 0, 0),
                EventType = "Cycle",
                RouteDistance = 40.00m
            };

            // Act
            var result = await controller.CreateEvent(newEvent);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
            Assert.Single(context.Events);
        }
    }
}
