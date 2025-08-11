using FGC.Application.Deals.Queries.GetAlldeals;
using FGC.Application.Deals.Queries.Getdeals;
using FGC.Application.UnitTests;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Deals;
using FGC.Domain.Entities.Games;
using FluentAssertions;

namespace FGC.Application.UnitTests.DealsTests.Queries.GetDealsTests
{
    public class GetDealsQueryTests : TestFixture
    {
        [Fact]
        public async Task Handle_ShouldReturnAllDeals_WhenDealsExist()
        {
            // Arrange
            base.Context.Deals.RemoveRange(base.Context.Deals);
            base.Context.Games.RemoveRange(base.Context.Games);
            await base.Context.SaveChangesAsync();


            var game1 = new Game
            {
                Name = "Game One",
                Category = "RPG",
                Price = new CurrencyAmount(100, "BRL")
            };

            var game2 = new Game
            {
                Name = "Game Two",
                Category = "Action",
                Price = new CurrencyAmount(200, "BRL")
            };

            var deal = new Deal(
                20,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(10),
                "deal of FPS games"
            );

            deal.Games.Add(game1);
            deal.Games.Add(game2);

            base.Context.Games.AddRange(game1, game2);
            base.Context.Deals.Add(deal);
            await base.Context.SaveChangesAsync();

            var handler = new GetDealsQueryCommandHandler(base.Context);
            var query = new GetDealsByIdQuery
            {
                Id = deal.Id
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.DealId.Should().Be(deal.Id);
            result.Description.Should().Be("deal of FPS games");
            result.Discount.Should().Be(20);
            result.StartDate.Should().Be(deal.StartDate);
            result.ExpirationDate.Should().Be(deal.ExpirationDate);
        }
    }
}
