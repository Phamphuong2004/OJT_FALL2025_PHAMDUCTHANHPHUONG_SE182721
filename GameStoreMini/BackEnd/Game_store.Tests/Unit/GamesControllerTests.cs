using System.Threading.Tasks;
using Xunit;
using GameStoreMini.Controllers;
using GameStoreMini.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using GameStoreMini.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Game_store.UnitTests
{
    public class GamesControllerTests
    {
        private AppDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task Create_NegativePrice_ReturnsBadRequest()
        {
            // Arrange
            await using var db = CreateDbContext("create_neg_price");
            var env = new Moq.Mock<IWebHostEnvironment>();
            var controller = new GamesController(db, env.Object);

            var dto = new CreateGameDto { Title = "x", Price = -1m, Stock = 0 };

            // Act
            var result = await controller.Create(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var bad = result as BadRequestObjectResult;
            Assert.Equal("Price and Stock must be non-negative.", bad?.Value);
        }

        [Fact]
        public async Task Update_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            await using var db = CreateDbContext("update_not_found");
            var env = new Moq.Mock<IWebHostEnvironment>();
            var controller = new GamesController(db, env.Object);

            var dto = new CreateGameDto { Title = "x", Price = 10m, Stock = 1 };

            // Act
            var result = await controller.Update(9999, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
