using FGC.Domain.Core.Exceptions;
using FGC.Domain.Entities.Deals;

namespace FGC.UnitTests.Domain.Entities
{
    public class DealsTest
    {
        [Fact]
        public void Constructor_ValidData_ShouldCreateDealsuccessfully()
        {
            // Arrange
            var discount = 15.5M;
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddDays(15);
            var description = "Counter strike deal";

            // Act
            var deal = new Deal(discount, startDate, endDate, description);

            // Assert
            Assert.Equal(discount, deal.Discount.Value);
            Assert.Equal(startDate, deal.StartDate);
            Assert.Equal(endDate, deal.ExpirationDate);
            Assert.Equal(description, deal.Description);
        }

        [Fact]
        public void IsExpired_ShouldReturnTrue_WhenCurrentDateIsAfterEndDate()
        {
            // Arrange
            var deal = new Deal(20.0M, DateTime.UtcNow.AddDays(-20), DateTime.UtcNow.AddDays(-1), "Expired deal");

            // Act
            var isExpired = deal.IsExpired();

            // Assert
            Assert.True(isExpired);
        }

        [Fact]
        public void IsActive_ShouldReturnTrue_WhenCurrentDateIsWithinStartAndEndDates()
        {
            // Arrange
            var deal = new Deal(10.0M, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(5), "Active deal");

            // Act
            var isActive = deal.IsActive();

            // Assert
            Assert.True(isActive);
        }

        [Fact]
        public void IsActive_ShouldReturnFalse_WhenStartDateIsInFuture()
        {
            // Arrange
            var deal = new Deal(10.0M, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(10), "Future deal");

            // Act
            var isActive = deal.IsActive();

            // Assert
            Assert.False(isActive);
        }

        [Fact]
        public void ValidatePeriod_ShouldThrow_WhenEndDateIsBeforeStartDate()
        {
            // Arrange
            var deal = new Deal(5.0M, DateTime.UtcNow, DateTime.UtcNow.AddDays(-2), "Invalid Period deal");

            // Act & Assert
            var ex = Assert.Throws<BusinessRulesException>(() => deal.ValidatePeriod());
            Assert.Equal("deal end date cannot be earlier than the start date.", ex.Message);
        }

        [Fact]
        public void GetDiscountPrice_ShouldReturnCorrectDiscountedPrice()
        {
            // Arrange
            var deal = new Deal(25.0M, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), "Quarter Off");
            var originalPrice = 200.0M;

            // Act
            var discountedPrice = deal.GetDiscountPrice(originalPrice);

            // Assert
            Assert.Equal(150.0M, discountedPrice);
        }

        [Fact]
        public void UpdateDiscount_ShouldUpdateDiscountAndExpirationDate()
        {
            // Arrange
            var deal = new Deal(5.0M, DateTime.UtcNow, DateTime.UtcNow.AddDays(3), "Limited Offer");
            var newDiscount = 12.0M;
            var newEndDate = DateTime.UtcNow.AddDays(10);

            // Act
            deal.UpdateDiscount(newDiscount, newEndDate, DateTime.UtcNow);

            // Assert
            Assert.Equal(newDiscount, deal.Discount.Value);
            Assert.Equal(newEndDate, deal.ExpirationDate);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenDiscountIsNegative()
        {
            // Arrange
            var discount = -5.0M;
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddDays(5);
            var description = "Invalid Negative Discount";

            // Act & Assert
            Assert.Throws<BusinessRulesException>(() => new Deal(discount, startDate, endDate, description));
        }

        [Fact]
        public void GetDiscountPrice_WithZeroPercentDiscount_ShouldReturnOriginalPrice()
        {
            // Arrange
            var deal = new Deal(0.0M, DateTime.UtcNow, DateTime.UtcNow.AddDays(5), "Zero Discount");
            var originalPrice = 100.0M;

            // Act
            var discountedPrice = deal.GetDiscountPrice(originalPrice);

            // Assert
            Assert.Equal(100.0M, discountedPrice);
        }

        [Fact]
        public void GetDiscountPrice_With100PercentDiscount_ShouldReturnZero()
        {
            // Arrange
            var deal = new Deal(100.0M, DateTime.UtcNow, DateTime.UtcNow.AddDays(3), "Everything Free!");
            var originalPrice = 250.0M;

            // Act
            var discountedPrice = deal.GetDiscountPrice(originalPrice);

            // Assert
            Assert.Equal(0.0M, discountedPrice);
        }
    }
}
