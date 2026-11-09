using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Controllers;
using RaceDay.Core.Data;
using RaceDay.Core.Entities;
using Xunit;

public class AuthControllerTests
{
    private RaceDayDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<RaceDayDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new RaceDayDbContext(options);
    }

    [Fact]
    public async Task Register_Returns_Created_When_Data_Is_Valid()
    {
        // Arrange
        var context = GetDatabaseContext();
        var controller = new AuthController(context);
        var dto = new RegisterDto("Sipho", "Dlamini", "sipho@example.com", "Password123", "0712345678", "Participant", null);

        // Act
        var result = await controller.Register(dto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, statusCodeResult.StatusCode);
    }
}
