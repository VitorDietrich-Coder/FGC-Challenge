using FGC.Application.Games.Queries.GetGames;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Games;
using FGC.Application.UnitTests;
using FGC.Application.Common.Exceptions;
using FluentAssertions;
using Xunit;
using Ardalis.GuardClauses;

namespace FGC.UnitTests.Application.GameTests.Queries.GetGamesTests
{
    public class GetGameByIdQueryTests : TestFixture
    {
        [Fact]
        public async Task Handle_ShouldReturnGame_WhenGameExists()
        {
            // Arrange
            var game = new Game
            {
                Name = "Test Game",
                Category = "RPG",
                Price = new CurrencyAmount(79.99m, "USD")
            };

            base.Context.Games.Add(game);
            await base.Context.SaveChangesAsync();

            var query = new GetGameByIdQuery { Id = game.Id };
            var handler = new GetGameQueryCommandHandler(base.Context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test Game");
            result.Category.Should().Be("RPG");
            result.Price.Should().Be(79.99m);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenGameDoesNotExist()
        {
            // Arrange
            var query = new GetGameByIdQuery { Id = 999 };
            var handler = new GetGameQueryCommandHandler(base.Context);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
