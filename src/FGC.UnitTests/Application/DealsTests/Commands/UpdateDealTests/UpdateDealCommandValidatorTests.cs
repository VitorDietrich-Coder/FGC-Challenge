using FGC.Application.Deals.Commands.UpdateDeal;
using FGC.Application.UnitTests;
using FluentValidation.TestHelper;

namespace FGC.Application.UnitTests.DealsTests.Commands.UpdateDealTests
{
    public class UpdateDealCommandValidatorTests : IClassFixture<TestFixture>
    {
        private readonly UpdateDealCommandValidator _validator;

        public UpdateDealCommandValidatorTests()
        {
            _validator = new UpdateDealCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Discount_Is_Zero()
        {
            var command = new UpdateDealCommand
            {
                Discount = 0,
                GameId = [1, 2, 3]
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Discount.Value");
        }

        [Fact]
        public void Should_Have_Error_When_Discount_Is_Over_100()
        {
            var command = new UpdateDealCommand
            {
                Discount = 150,
                GameId = [1, 2, 3]
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Discount.Value");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Discount_Is_Valid()
        {
            var command = new UpdateDealCommand
            {
                Discount = 25,
                GameId = [1, 2, 3]
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor("Discount.Value");
        }

 
        [Fact]
        public void Should_Not_Have_Error_When_ExpirationDate_Is_Valid()
        {
            var command = new UpdateDealCommand
            {
                ExpirationDate = DateTime.UtcNow.AddDays(1),
                GameId = [1, 2, 3]
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor("ExpirationDate.Value");
        }

        [Fact]
        public void Should_Have_Error_When_GameId_Is_Empty()
        {
            var command = new UpdateDealCommand
            {
                GameId = new List<int?>()
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.GameId);
        }

        [Fact]
        public void Should_Have_Error_When_GameId_Contains_Invalid_Id()
        {
            var command = new UpdateDealCommand
            {
                GameId = new List<int?> { 1, null, -2 }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.GameId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_GameId_Is_Valid()
        {
            var command = new UpdateDealCommand
            {
                GameId = new List<int?> { 1, 2, 3 }
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.GameId);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            var command = new UpdateDealCommand
            {
                ExpirationDate = DateTime.UtcNow.AddDays(12),
                Discount = 1,
                Description = "",
                GameId = [1 , 2, 3]
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Too_Short()
        {
            var command = new UpdateDealCommand
            {
                Description = "Short",
                GameId = [1, 2, 3]
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Too_Long()
        {
            var command = new UpdateDealCommand
            {
                Description = new string('a', 101),
                GameId = [1, 2, 3]
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Description_Is_Valid()
        {
            var command = new UpdateDealCommand
            {
                GameId = [1],
                Description = "This is a valid description"
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }
    }
}
