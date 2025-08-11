using FGC.Application.UnitTests;
using FGC.Application.Users.Commands.UpdateUser;
using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.Enums;
using FGC.Domain.Entities.Users.ValueObjects;
using FluentAssertions;
using Xunit;

namespace FGC.Application.UnitTests.UserTests.Commands.UpdateUserTests
{
    public class UpdateUserCommandTests : TestFixture
    {
        [Fact]
        public async Task Handle_ShouldUpdateUser_WhenDataIsValid()
        {
            // Arrange
            var user = new User
            {
                Name = "Old Name",
                Username = "oldusername",
                Email = new Email("old@example.com"),
                Password = new Password("Old@1234"),
                TypeUser = UserType.User,
                Active = false
            };

            base.Context.Users.Add(user);
            await base.Context.SaveChangesAsync();

            var command = new UpdateUserCommand
            {
                Id = user.Id,
                Name = "New Name",
                Username = "oldusername",
                Email = "new@example.com",
                Password = "NewPass@123",
                TypeUser = UserType.Admin,
                Active = true
            };

            var handler = new UpdateUserCommandHandler(base.Context);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedUser = base.Context.Users.FirstOrDefault(u => u.Id == user.Id);
            updatedUser.Should().NotBeNull();
            updatedUser.Name.Should().Be(command.Name);
            updatedUser.Email.Address.Should().Be(command.Email);
            updatedUser.TypeUser.Should().Be(command.TypeUser);
            updatedUser.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
