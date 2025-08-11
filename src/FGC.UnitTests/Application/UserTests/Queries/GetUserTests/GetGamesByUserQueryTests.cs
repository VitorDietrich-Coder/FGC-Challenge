using FGC.Application.Users.Queries.GetUser;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Games;
using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.Enums;
using FGC.Domain.Entities.Users.ValueObjects;
using FGC.Application.UnitTests;
using FluentAssertions;
using Xunit;

namespace FGC.Application.UnitTests.UserTests.Queries.GetUserTests
{
    public class GetGamesByUserQueryTests : TestFixture
    {
        [Fact]
        public async Task Handle_ShouldReturnUserGames_WhenUserHasGames()
        {
            // Arrange
            base.Context.Users.RemoveRange(base.Context.Users);
            var user = new User
            {
                Name = "Test User",
                Username = "testuser",
                Email = new Email("test@example.com"),
                Password = new Password("Password@123"),
                TypeUser = UserType.User,
                Active = true
            };

            var game = new Game
            {
                Name = "Test Game",
                Category = "RPG",
                Price = new CurrencyAmount(100, "USD"),
            };

            base.Context.Users.Add(user);
            base.Context.Games.Add(game);
            await base.Context.SaveChangesAsync();

            var library = new UserGameLibrary
            {
                UserId = user.Id,
                GameId = game.Id,
                FinalPrice = new CurrencyAmount(70, "USD"),
                DateOfPurchase = DateTime.UtcNow,
            };

            base.Context.UserGamesLibrary.Add(library);
            await base.Context.SaveChangesAsync();

            var query = new GetGamesByUserQuery { Id = user.Id };
            var handler = new GetGamesByUserQueryCommandHandler(base.Context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            var gameResult = result.First();
            gameResult.Name.Should().Be("Test Game");
            gameResult.BasePrice.Should().Be(100);
            gameResult.FinalPrice.Should().Be(70);
            gameResult.DiscountAmount.Should().Be(30);
            gameResult.DiscountPercentage.Should()
                .BeInRange(30.0m - 0.01m, 30.0m + 0.01m);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenUserHasNoGames()
        {
            // Arrange
            var user = new User
            {
                Name = "No Games User",
                Username = "nogames",
                Email = new Email("no@games.com"),
                Password = new Password("Password@123"),
                TypeUser = UserType.User,
                Active = true
            };

            base.Context.Users.Add(user);
            await base.Context.SaveChangesAsync();

            var query = new GetGamesByUserQuery { Id = user.Id };
            var handler = new GetGamesByUserQueryCommandHandler(base.Context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
