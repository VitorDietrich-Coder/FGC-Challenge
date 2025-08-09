using Abp.Application.Features;
using FGC.Application.Common;
using FGC.Application.Games.Commands.CreateGame;
using FGC.Application.UnitTests;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Games;
using FGC.Infra.Data;
using FluentValidation.TestHelper;

namespace FGC.Application.UnitTests.GamesTests.Commands.CreateGameTests
{
    public class CreateGameCommandValidatorTests : IClassFixture<TestFixture>
    {
        private readonly CreateGameCommandValidator _validator;
        private readonly ApplicationDbContext _applicationDbContext;

        public CreateGameCommandValidatorTests(TestFixture fixture)
        {
            _validator = new CreateGameCommandValidator(fixture.Context);
            _applicationDbContext = fixture.Context;
        }

        [Fact]
        public async void Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new CreateGameCommand
            {
                Name = ""
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async void Should_Have_Error_When_Name_Contains_Invalid_Characters()
        {
            var command = new CreateGameCommand
            {
                Name = "Invalid#Name!"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async void Should_Have_Error_When_Name_Is_Too_Long()
        {
            var command = new CreateGameCommand
            {
                Name = new string('a', 101)
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async Task Should_Have_Error_When_Name_Already_Exists()
        {
            var existingGameName = "Existing Game";

            _applicationDbContext.Games.Add(new  Game { Name = existingGameName, Category ="teste", Price = new  CurrencyAmount(20M, "BRL") });
            await _applicationDbContext.SaveChangesAsync(CancellationToken.None);

            var command = new CreateGameCommand
            {
                Name = existingGameName,
                Category = "Action",
                Price = 50,
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async void Should_Not_Have_Error_When_Name_Is_Valid()
        {
            var command = new CreateGameCommand
            {
                Name = "Valid Game Name"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async void Should_Have_Error_When_Category_Is_Empty()
        {
            var command = new CreateGameCommand
            {
                Name = "teste",
                Category = ""
            };

            var result =  await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Category);
        }

        [Fact]
        public async void Should_Have_Error_When_Category_Contains_Invalid_Characters()
        {
            var command = new CreateGameCommand
            {
                Category = "Action123!"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Category);
        }

        [Fact]
        public async void Should_Have_Error_When_Category_Is_Too_Long()
        {
            var command = new CreateGameCommand
            {
                Category = new string('a', 101)
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Category);
        }

        [Fact]
        public async void Should_Not_Have_Error_When_Category_Is_Valid()
        {
            var command = new CreateGameCommand
            {
                Category = "Adventure"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Category);
        }

        [Fact]
        public async void Should_Have_Error_When_Price_Is_Zero()
        {
            var command = new CreateGameCommand
            {
                Price = 0
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public async void Should_Have_Error_When_Price_Is_Negative()
        {
            var command = new CreateGameCommand
            {
                Price = -10
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public async void Should_Have_Error_When_Price_Is_Too_High()
        {
            var command = new CreateGameCommand
            {
                Price = 1001
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public async void Should_Not_Have_Error_When_Price_Is_Valid()
        {
            var command = new CreateGameCommand
            {
                Price = 59.99m
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public async void Should_Have_Error_When_DealId_Is_Invalid()
        {
            var command = new CreateGameCommand
            {
                DealId = -1
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.DealId);
        }

        [Fact]
        public async void Should_Not_Have_Error_When_DealId_Is_Valid()
        {
            var command = new CreateGameCommand
            {
                DealId = 5
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.DealId);
        }

        [Fact]
        public async void Should_Not_Have_Error_When_DealId_Is_Null()
        {
            var command = new CreateGameCommand
            {
                DealId = null
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.DealId);
        }
    }
}
