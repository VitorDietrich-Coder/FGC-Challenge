using FGC.Domain.Core.Exceptions;
using FGC.Domain.Entities.Games;
using FGC.Domain.Entities.Sales;
using FGC.Domain.Entities.Users;

namespace FGC.Unit.Tests.Domain.Entities
{
    public class GamesTest
    {
        [Fact]
        public void ParameterlessConstructor_ShouldCreateInstance()
        {
            var game = new Game();

            Assert.NotNull(game);
            Assert.Null(game.Name);
            Assert.Null(game.Category);
            Assert.Equal(null, game.Price);
            Assert.Null(game.SaleID);
            Assert.Null(game.Sale);
            Assert.Null(game.Libraries);
        }

        [Fact]
        public void Constructor_WithBasicParameters_ShouldSetProperties()
        {
            var game = new Game("Game1", "Action", 49.99M, null);

            Assert.Equal("Game1", game.Name);
            Assert.Equal("Action", game.Category);
            Assert.Equal(49.99M, game.Price.Value);
            Assert.Equal("BRL", game.Price.Currency);
            Assert.Null(game.SaleID);
        }

        [Fact]
        public void Constructor_WithId_ShouldSetAllProperties()
        {
            var game = new Game(10, "Game With ID", "RPG", 59.99M, 5);

            Assert.Equal(10, game.Id);
            Assert.Equal("Game With ID", game.Name);
            Assert.Equal("RPG", game.Category);
            Assert.Equal(59.99M, game.Price.Value);            
            Assert.Equal(5, game.SaleID);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(-9999999999999999)]
        public void Constructor_ShouldThrowException_WhenPriceIsNegative(decimal price)
        {
            var ex = Assert.Throws<BusinessRulesException>(() =>
                new Game("Game", "Action", price, null));

            Assert.Equal("The monetary amount must be greater than or equal to zero.", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldAllow_WhenPriceIsZero()
        {
            var game = new Game("Free Game", "Indie", 0, null);

            Assert.Equal(0, game.Price.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowException_WhenNameIsInvalid(string name)
        {
            var ex = Assert.Throws<BusinessRulesException>(() =>
                new Game(name, "Action", 49.99M, null));

            Assert.Equal("Game name is required.", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowException_WhenGenreIsInvalid(string genre)
        {
            var ex = Assert.Throws<BusinessRulesException>(() =>
                new Game("Game", genre, 49.99M, null));

            Assert.Equal("Category game is required.", ex.Message);
        }

        [Fact]
        public void AssignPromotion_ShouldSetPromotionId()
        {
            var game = new Game("Game", "Puzzle", 19.99M, null);
            game.UpdateSale(5);

            Assert.Equal(5, game.SaleID);
        }

        [Fact]
        public void Promotion_Property_ShouldWorkCorrectly()
        {
            var promotion = new Sale(20, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), "description");
            var game = new Game { Sale = promotion };

            Assert.NotNull(game.Sale);
            Assert.Equal(20, game.Sale.Discount.Value);
        }

        [Fact]
        public void PromotionId_ShouldWork_WithoutPromotionObject()
        {
            var game = new Game { SaleID = 5 };

            Assert.Equal(5, game.SaleID);
            Assert.Null(game.Sale);
        }

        [Fact]
        public void Should_Handle_Null_Promotion()
        {
            var game = new Game { Sale = null };

            Assert.Null(game.Sale);
        }

        [Fact]
        public void Libraries_ShouldInitialize_WhenAssigned()
        {
            var game = new Game();
            game.Libraries = new List<UserGameLibrary> { new() };

            Assert.NotNull(game.Libraries);
            Assert.Single(game.Libraries);
        }

        [Fact]
        public void Libraries_ShouldBeNull_ByDefault()
        {
            var game = new Game();

            Assert.Null(game.Libraries);
        }

        [Fact]
        public void Should_Add_To_Libraries()
        {
            var game = new Game { Libraries = new List<UserGameLibrary>() };
            var libraryGame = new UserGameLibrary();

            game.Libraries.Add(libraryGame);

            Assert.Contains(libraryGame, game.Libraries);
        }

        [Fact]
        public void Constructor_ShouldHandle_MaxDoublePrice()
        {
            var game = new Game("Expensive Game", "AAA", decimal.MaxValue, null);

            Assert.Equal(decimal.MaxValue, game.Price.Value);
        }

        [Fact]
        public void Constructor_ShouldHandle_LongStrings()
        {
            var game = new Game(new string('A', 1000), new string('B', 500), 59.99M, null);

            Assert.Equal(1000, game.Name.Length);
            Assert.Equal(500, game.Category.Length);
        }

        [Fact]
        public void ValidatePrice_ShouldPass_ForValidValues()
        {
            var game1 = new Game("Game1", "Action", 0, null);
            var game2 = new Game("Game2", "Action", 10.5M, null);
            var game3 = new Game("Game3", "Action", decimal.MaxValue, null);

            Assert.Equal(0, game1.Price.Value);
            Assert.Equal(10.5M, game2.Price.Value);
            Assert.Equal(decimal.MaxValue, game3.Price.Value);
        }

        [Fact]
        public void Libraries_ShouldBeInitializedBeforeUse()
        {
            var game = new Game();

            Assert.Null(game.Libraries);

            game.Libraries = new List<UserGameLibrary>();

            Assert.NotNull(game.Libraries);
            Assert.Empty(game.Libraries);
        }

        [Fact]
        public void Promotion_ShouldUpdateCorrectly_WhenReassigned()
        {
            var game = new Game();
            var promotion1 = new Sale(10, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), "description");
            var promotion2 = new Sale(20, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "description");

            game.Sale = promotion1;
            game.Sale = promotion2;

            Assert.Equal(20, game.Sale.Discount.Value);
        }

        [Fact]
        public void Libraries_Setter_ShouldAcceptNewCollection()
        {
            var game = new Game();
            var newLibraries = new List<UserGameLibrary> { new UserGameLibrary() };

            game.Libraries = newLibraries;

            Assert.NotNull(game.Libraries);
            Assert.Single(game.Libraries);
        }

        [Fact]
        public void Promotion_Setter_ShouldHandleReassignment()
        {
            var game = new Game();
            var sale1 = new Sale(15, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), "description");
            var sale2 = new Sale(35, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "description");

            game.Sale = sale1;
            game.Sale = sale2;

            Assert.Equal(35, game.Sale.Discount.Value);
        }

        [Fact]
        public void CreateGame_ShouldSetPriceWithCurrency()
        {
            // Arrange
            var name = "Test Game";
            var category = "Action";
            var price = 99.99M;
            var saleId = (int?)null;

            // Act
            var game = new Game(name, category, price, saleId);

            // Assert
            Assert.Equal(name, game.Name);
            Assert.Equal(category, game.Category);
            Assert.Equal(price, game.Price.Value);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenCurrencyIsInvalid()
        {
               HashSet<string> ValidCurrencies = new HashSet<string>
               {
                   "USD", // US Dollar
                   "EUR", // Euro
                   "BRL", // Brazilian Real
                   "JPY", // Japanese Yen
                   "GBP", // British Pound
                   "AUD", // Australian Dollar
                   "CAD", // Canadian Dollar
                   "CHF", // Swiss Franc
                   "CNY", // Chinese Yuan
                   "SEK", // Swedish Krona
                   "NZD"  // New Zealand Dollar
               };
               var ex = Assert.Throws<BusinessRulesException>(() =>
                        new Game("Game", "Action", 49.99M, null, "INVALID"));

            Assert.Equal($"Invalid currency: 'INVALID'. Supported currencies are: {string.Join(", ", ValidCurrencies)}.", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldAllow_WhenCurrencyIsValid()
        {
            var game = new Game("Game", "Action", 49.99M, null, "USD");

            Assert.Equal(49.99M, game.Price.Value);
            Assert.Equal("USD", game.Price.Currency);
        }

        [Fact]
        public void CreateGame_ShouldSetPriceWithValidCurrency()
        {
            // Arrange
            var name = "Test Game";
            var genre = "Action";
            var price = 99.99M;
            var saleId = (int?)null;
            var currency = "USD";

            // Act
            var game = new Game(name, genre, price, saleId, currency);

            // Assert
            Assert.Equal(name, game.Name);
            Assert.Equal(genre, game.Category);
            Assert.Equal(price, game.Price.Value);
            Assert.Equal(currency, game.Price.Currency);
        }
    }
}
