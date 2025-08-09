using FGC.Application.Games.Commands.CreateGame;
using FGC.Domain.Common.ValueObjects;
using FluentAssertions;
using Xunit;

namespace FGC.Application.UnitTests.GamesTests.Commands.CreateGameTests
{
    public class CreateGameCommandTests : TestFixture
    {
        [Fact]
        public async Task Handle_ShouldCreateGame_WhenDataIsValid()
        {
            // Arrange
            var command = new CreateGameCommand
            {
                Name = "Halo Infinite",
                Category = "Shooter",
                Price = 59.99m
            };

            var handler = new CreateGameCommandHandler(base.Context);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(command.Name);
            result.Category.Should().Be(command.Category);
            result.Price.Should().Be(command.Price);
            result.DealId.Should().BeNull();

            var createdGame = base.Context.Games.FirstOrDefault(g => g.Name == command.Name);
            createdGame.Should().NotBeNull();
            createdGame!.Category.Should().Be(command.Category);
            createdGame.Price.Value.Should().Be(command.Price);
        }

        [Fact]
        public async Task Handle_ShouldCreateGame_WithDeal_WhenDealIdProvided()
        {
            // Arrange
            var deal = new Domain.Entities.Deals.Deal
            {
                Discount = new CurrencyAmount(20),
                ExpirationDate = DateTime.UtcNow.AddDays(5),
                Description = "Summer Deal"
            };

            base.Context.Deals.Add(deal);
            await base.Context.SaveChangesAsync();

            var command = new CreateGameCommand
            {
                Name = "The Witcher 3",
                Category = "RPG",
                Price = 39.99m,
                DealId = deal.Id
            };

            var handler = new CreateGameCommandHandler(base.Context);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.DealId.Should().Be(deal.Id);

            var createdGame = base.Context.Games.FirstOrDefault(g => g.Name == command.Name);
            createdGame.Should().NotBeNull();
            createdGame!.DealId.Should().Be(deal.Id);
        }
    }
}
