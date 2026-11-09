using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Controllers;
using RaceDay.Core.Data;
using RaceDay.Core.Entities;
using Xunit;

namespace RaceDay.Tests.Controllers
{
    public class AuthControllerTests
    {
        private RaceDayDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new RaceDayDbContext(options);
        }

        [Fact]
        public async Task Register_NewUser_ReturnsCreatedResult()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new AuthController(context);
            var dto = new RegisterDto(
                FirstName: "Sipho",
                LastName: "Dlamini",
                Email: "sipho.test@example.com",
                Password: "SecurePassword123",
                PhoneNumber: "0734567890",
                Role: "Participant",
                OrganisationName: null
            );

            // Act
            var result = await controller.Register(dto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
            Assert.Single(context.Users);
        }

        [Fact]
        public async Task Register_ExistingEmail_ReturnsConflict()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Users.Add(new User
            {
                FirstName = "Existing",
                LastName = "User",
                Email = "duplicate@example.com",
                PasswordHash = "hash",
                Role = "Participant"
            });
            await context.SaveChangesAsync();

            var controller = new AuthController(context);
            var dto = new RegisterDto(
                FirstName: "Another",
                LastName: "User",
                Email: "duplicate@example.com",
                Password: "Password123",
                PhoneNumber: null,
                Role: "Participant",
                OrganisationName: null
            );

            // Act
            var result = await controller.Register(dto);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new AuthController(context);
            var dto = new LoginDto("wrong@example.com", "BadPassword");

            // Act
            var result = await controller.Login(dto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
