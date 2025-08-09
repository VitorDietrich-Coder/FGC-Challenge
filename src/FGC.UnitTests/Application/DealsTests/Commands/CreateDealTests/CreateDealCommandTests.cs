using FGC.Application.Deals.Commands.CreateDeal;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Games;
using FluentAssertions;
using Xunit;

namespace FGC.Application.UnitTests.DealsTests.Commands.CreateDealTests;

public class CreateDealCommandTests : TestFixture
{
    [Fact]
    public async Task Handle_ShouldCreateDeal_WhenDataIsValid()
    {
        // Arrange
        var game1 = new Game
        {
            Name = "Game 1",
            Category = "Action",
            Price = new CurrencyAmount(100, "USD")
        };

        var game2 = new Game
        {
            Name = "Game 2",
            Category = "Adventure",
            Price = new CurrencyAmount(200, "USD")
        };

        base.Context.Games.AddRange(game1, game2);
        await base.Context.SaveChangesAsync();

        var command = new CreateDealCommand
        {
            Discount = 30,
            ExpirationDate = DateTime.UtcNow.AddDays(10),
            GameId = new List<int?> { game1.Id, game2.Id },
            Description = "deal of FPS games"
        };

        var handler = new CreateDealCommandHandler(base.Context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        var createdDeal = base.Context.Deals.FirstOrDefault(d => d.Description == command.Description);
        createdDeal.Should().NotBeNull();
        createdDeal!.Games.Should().HaveCount(2);
        createdDeal.Discount.Value.Should().Be(command.Discount);
    }
}
