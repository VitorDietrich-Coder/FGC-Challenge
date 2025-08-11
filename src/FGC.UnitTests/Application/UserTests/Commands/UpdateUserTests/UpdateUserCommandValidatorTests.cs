
using FGC.Application.UnitTests;
using FGC.Application.Users.Commands.UpdateUser;
using FGC.Domain.Entities.Users.Enums;
using FluentValidation.TestHelper;

namespace FGC.Application.UnitTests.UserTests.Commands.UpdateUserTests
{
    public class UpdateUserCommandValidatorTests : IClassFixture<TestFixture>
    {
        private readonly UpdateUserValidator _validator;

        public UpdateUserCommandValidatorTests()
        {
            _validator = new UpdateUserValidator();
        }

        [Fact]
        public async Task Should_Have_Error_When_All_Fields_Are_Null()
        {
            var command = new UpdateUserCommand();

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x)
                .WithErrorMessage("At least one field must be provided.");
        }

        [Fact]
        public async Task Should_Have_Error_When_Name_Is_Too_Long()
        {
            var command = new UpdateUserCommand
            {
                Name = new string('a', 101)
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Name_Is_Valid()
        {
            var command = new UpdateUserCommand
            {
                Name = "Updated User"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Invalid()
        {
            var command = new UpdateUserCommand
            {
                Email = "invalid-email"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Too_Long()
        {
            var command = new UpdateUserCommand
            {
                Email = new string('e', 101) + "@test.com"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Email_Is_Valid()
        {
            var command = new UpdateUserCommand
            {
                Email = "user@example.com"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Should_Have_Error_When_Password_Is_Too_Short()
        {
            var command = new UpdateUserCommand
            {
                Password = "A1@"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public async Task Should_Have_Error_When_Password_Is_Too_Long()
        {
            var command = new UpdateUserCommand
            {
                Password = new string('A', 101) + "1@"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Theory]
        [InlineData("password123")] // no special character
        [InlineData("PASSWORD@")]   // no number
        [InlineData("12345678@")]   // no letter
        public async Task Should_Have_Error_When_Password_Does_Not_Meet_Complexity(string password)
        {
            var command = new UpdateUserCommand
            {
                Password = password
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Password_Is_Valid()
        {
            var command = new UpdateUserCommand
            {
                Password = "Abc@1234"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public async Task Should_Have_Error_When_TypeUser_Is_Invalid()
        {
            var command = new UpdateUserCommand
            {
                TypeUser = (UserType)999
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.TypeUser);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_TypeUser_Is_Valid()
        {
            var command = new UpdateUserCommand
            {
                TypeUser = UserType.Admin
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.TypeUser);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_Not_Have_Error_When_Active_Is_Valid(bool active)
        {
            var command = new UpdateUserCommand
            {
                Active = active
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Active);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_At_Least_One_Field_Is_Provided()
        {
            var command = new UpdateUserCommand
            {
                Name = "Someone"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x);
        }
    }
}
