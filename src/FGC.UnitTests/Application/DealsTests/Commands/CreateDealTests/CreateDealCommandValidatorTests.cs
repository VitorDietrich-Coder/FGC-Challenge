using FGC.Application.Deals.Commands.CreateDeal;
using FGC.Application.UnitTests;
using FluentValidation.TestHelper;
using Xunit;

namespace FGC.UnitTests.Application.DealsTests.Commands.CreateDealTests
{
    public class CreateDealCommandValidatorTests : IClassFixture<TestFixture>
    {
        private readonly CreatedealCommandValidator _validator;

        public CreateDealCommandValidatorTests()
        {
            _validator = new CreatedealCommandValidator();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Should_Have_Error_When_Discount_Is_Invalid(decimal discount)
        {
            var command = new CreateDealCommand { Discount = discount };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Discount);
        }

        [Fact]
        public void Should_Have_Error_When_Discount_Is_Empty()
        {
            var command = new CreateDealCommand();
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Discount);
        }

        [Fact]
        public void Should_Pass_When_Discount_Is_Valid()
        {
            var command = new CreateDealCommand { Discount = 50 };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Discount);
        }

        [Fact]
        public void Should_Have_Error_When_ExpirationDate_Is_In_Past()
        {
            var command = new CreateDealCommand { ExpirationDate = DateTime.UtcNow.AddDays(-1) };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.ExpirationDate);
        }

        [Fact]
        public void Should_Pass_When_ExpirationDate_Is_In_Future()
        {
            var command = new CreateDealCommand { ExpirationDate = DateTime.UtcNow.AddDays(1) };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.ExpirationDate);
        }

        [Fact]
        public void Should_Have_Error_When_GameIds_Are_Invalid()
        {
            var command = new CreateDealCommand
            {
                GameId = new List<int?> { 1, null, -2 }
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.GameId);
        }

        [Fact]
        public void Should_Pass_When_GameIds_Are_Valid()
        {
            var command = new CreateDealCommand
            {
                GameId = new List<int?> { 1, 2, 3 }
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.GameId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("short")]
         public void Should_Have_Error_When_Description_Is_Invalid(string description)
        {
            var command = new CreateDealCommand { Description = description };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Pass_When_Description_Is_Valid()
        {
            var command = new CreateDealCommand { Description = "Valid description for deal" };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }
    }
}
