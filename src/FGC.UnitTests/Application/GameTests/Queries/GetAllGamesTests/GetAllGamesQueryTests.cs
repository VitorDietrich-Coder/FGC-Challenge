using FGC.Application.Games.Queries.GetAllGames;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Games;
using FGC.Application.UnitTests;
using FluentAssertions;
using Xunit;

namespace FGC.UnitTests.Application.GameTests.Queries.GetAllGamesTests
{
    public class GetAllGamesQueryTests : TestFixture
    {
        [Fact]
        public async Task Handle_ShouldReturnAllGames()
        {
            base.Context.Deals.RemoveRange(base.Context.Deals);
            base.Context.Games.RemoveRange(base.Context.Games);
            await base.Context.SaveChangesAsync();

            // Arrange
            var game1 = new Game
            {
                Name = "Game One",
                Category = "Action",
                Price = new CurrencyAmount(59.99m, "USD")
            };

            var game2 = new Game
            {
                Name = "Game Two",
                Category = "Adventure",
                Price = new CurrencyAmount(39.99m, "USD")
            };

            base.Context.Games.AddRange(game1, game2);
            await base.Context.SaveChangesAsync();

            var query = new GetAllGamesQuery();
            var handler = new GetUserQueryCommandHandler(base.Context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(g => g.Name == "Game One");
            result.Should().Contain(g => g.Name == "Game Two");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoGamesExist()
        {
            base.Context.Deals.RemoveRange(base.Context.Deals);
            base.Context.Games.RemoveRange(base.Context.Games);
            await base.Context.SaveChangesAsync();

            // Arrange
            var query = new GetAllGamesQuery();
            var handler = new GetUserQueryCommandHandler(base.Context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
