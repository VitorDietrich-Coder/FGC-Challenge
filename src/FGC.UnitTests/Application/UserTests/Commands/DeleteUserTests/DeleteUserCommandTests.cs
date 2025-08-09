using Ardalis.GuardClauses;
using FGC.Application.UnitTests;
using FGC.Application.Users.Commands.DeleteUser;
using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.Enums;
using FGC.Domain.Entities.Users.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FGC.UnitTests.Application.UserTests.Commands.UpdateUserTests
{
    public class DeleteUserCommandTests : TestFixture
    {
        [Fact]
        public async Task Handle_ShouldDeleteUser_WhenIdIsValid()
        {
            // Arrange
            var user = new User
            {
                Name = "Delete Me",
                Username = "deleteme",
                Email = new Email("deleteme@example.com"),
                Password = new Password("Delete@123"),
                TypeUser = UserType.User,
                Active = true
            };

            base.Context.Users.Add(user);
            await base.Context.SaveChangesAsync();

            var command = new DeleteUserCommand
            {
                Id = user.Id
            };

            var handler = new DeleteUserCommandHandler(base.Context);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var deletedUser = await base.Context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            deletedUser.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenUserNotFound()
        {
            // Arrange
            var invalidId = 999;
            var command = new DeleteUserCommand { Id = invalidId };
            var handler = new DeleteUserCommandHandler(base.Context);

            // Act & Assert
            var act = async () => await handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"*{invalidId}*");
        }
    }
}
