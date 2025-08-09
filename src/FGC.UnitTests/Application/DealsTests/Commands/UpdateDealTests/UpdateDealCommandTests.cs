using FGC.Application.Deals.Commands.UpdateDeal;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Deals;
using FGC.Domain.Entities.Games;
using FluentAssertions;

namespace FGC.Application.UnitTests.DealsTests.Commands.UpdateDealTests
{
    public class UpdateDealCommandTests : TestFixture
    {
        [Fact]
        public async Task Handle_ShouldUpdateDeal_WhenDataIsValid()
        {
            // Arrange
            var originalGame = new Game
            {
                Name = "Old Game",
                Category = "Action",
                Price = new CurrencyAmount(100, "USD")
            };

            var newGame = new Game
            {
                Name = "New Game",
                Category = "Adventure",
                Price = new CurrencyAmount(150, "USD")
            };

            base.Context.Games.AddRange(originalGame, newGame);
            await base.Context.SaveChangesAsync();

            var deal = new Deal(
                30,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(10),
                "deal of FPS games"
            );

            deal.Games.Add(originalGame);

            base.Context.Deals.Add(deal);
            await base.Context.SaveChangesAsync();

            var command = new UpdateDealCommand
            {
                Id = deal.Id,
                Discount = 50,
                ExpirationDate = DateTime.UtcNow.AddDays(10),
                Description = "Updated Deal",
                GameId = new List<int?> { newGame.Id }
            };

            var handler = new UpdateDealCommandHandler(base.Context);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedDeal = base.Context.Deals.FirstOrDefault(d => d.Id == deal.Id);

            updatedDeal.Should().NotBeNull();
            updatedDeal!.Description.Should().Be("Updated Deal");
            updatedDeal.Discount.Value.Should().Be(50);
            updatedDeal.ExpirationDate.Should().Be(command.ExpirationDate);
            updatedDeal.Games.Should().ContainSingle(g => g.Id == newGame.Id);
        }
    }
}
