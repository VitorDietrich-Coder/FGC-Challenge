using FGC.Application.Users.Commands.CreateAdmin;
using FluentValidation.TestHelper;

namespace FGC.Application.UnitTests.UserTests.Commands.CreateAdminTests
{
    public class CreateAdminCommandValidatorTests : IClassFixture<TestFixture>
    {
        private readonly CreateAdminValidator _validator;

        public CreateAdminCommandValidatorTests()
        {
            _validator = new CreateAdminValidator();
        }

        [Fact]
        public async Task Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new CreateAdminCommand
            {
                Name = ""
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async Task Should_Have_Error_When_Name_Is_Too_Long()
        {
            var command = new CreateAdminCommand
            {
                Name = new string('a', 101)
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Name_Is_Valid()
        {
            var command = new CreateAdminCommand
            {
                Name = "Valid Name"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async Task Should_Have_Error_When_Username_Is_Too_Long()
        {
            var command = new CreateAdminCommand
            {
                Username = new string('u', 101)
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Username_Is_Null_Or_Empty()
        {
            var command = new CreateAdminCommand
            {
                Username = ""
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Empty()
        {
            var command = new CreateAdminCommand
            {
                Email = ""
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Invalid()
        {
            var command = new CreateAdminCommand
            {
                Email = "invalid-email"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Too_Long()
        {
            var command = new CreateAdminCommand
            {
                Email = new string('e', 101) + "@test.com"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Email_Is_Valid()
        {
            var command = new CreateAdminCommand
            {
                Email = "admin@example.com"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Should_Have_Error_When_Password_Is_Empty()
        {
            var command = new CreateAdminCommand
            {
                Password = ""
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public async Task Should_Have_Error_When_Password_Is_Too_Short()
        {
            var command = new CreateAdminCommand
            {
                Password = "Ab1@"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public async Task Should_Have_Error_When_Password_Is_Too_Long()
        {
            var command = new CreateAdminCommand
            {
                Password = new string('A', 101) + "1@"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Theory]
        [InlineData("Password123")]     
        [InlineData("password")]     
        [InlineData("PASSWORD@")]    
        [InlineData("Pass@word")]      
        public async Task Should_Have_Error_When_Password_Does_Not_Match_Complexity(string password)
        {
            var command = new CreateAdminCommand
            {
                Name = "teste",
                Email = "tste@teste.com",
                Username = "teste gameplays",
                Password = password
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Password_Is_Valid()
        {
            var command = new CreateAdminCommand
            {
                Password = "Valid@123"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }
    }
}
