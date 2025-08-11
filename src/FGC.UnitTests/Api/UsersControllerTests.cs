using FGC.Api.Controllers;
using FGC.Application.UnitTests;
using FGC.Application.Users.Commands.CreateUser;
using FGC.Application.Users.Commands.DeleteUser;
using FGC.Application.Users.Commands.UpdateUser;
using FGC.Application.Users.Commands.UpdateUser.ReleaseUserGame;
using FGC.Application.Users.Models.Response;
using FGC.Application.Users.Queries.GetAll;
using FGC.Application.Users.Queries.GetUser;
using FGC.Domain.Entities.Users.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FGC.Api.UnitTests.Login
{
    public class UsersControllerTests : TestFixture
    {
        private readonly UsersController _controller;
        private readonly Mock<ISender> _mediatorMock;

        public UsersControllerTests()
        {
            _mediatorMock = new Mock<ISender>();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(_mediatorMock.Object)
                .BuildServiceProvider();

            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            _controller = new UsersController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedUser()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Email = "user@test.com",
                Password = "Test123@",
                Username = "testuser"
            };

            var expectedUser = new UserResponse
            {
                UserId = 1,
                Email = command.Email,
                Username = command.Username
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<CreateUserCommand>(c => c.Email == command.Email), default))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.CreateAsync(command);

            // Assert
            var value = Assert.IsType<ActionResult<UserResponse>>(result);
            Assert.Equal(expectedUser.UserId, value.Value.UserId);
            Assert.Equal(expectedUser.Email, value.Value.Email);
        }

        [Fact]
        public async Task CreateAdminAsync_ShouldReturnCreatedAdminUser()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Email = "admin@test.com",
                Password = "Admin123@",
                Username = "adminuser"
            };

            var expectedUser = new UserResponse
            {
                UserId = 2,
                Email = command.Email,
                Username = command.Username
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<CreateUserCommand>(c => c.Email == command.Email), default))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.CreateAdminAsync(command);

            // Assert
            var value = Assert.IsType<ActionResult<UserResponse>>(result);
            Assert.Equal(expectedUser.UserId, value.Value.UserId);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnUser_WhenIdIsValid()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new UserResponse
            {
                UserId = userId,
                Email = "user@test.com",
                Username = "user"
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetUserQuery>(q => q.Id == userId), default))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.GetAsync(userId);

            // Assert
            var value = Assert.IsType<ActionResult<UserResponse>>(result);
            Assert.Equal(expectedUser.UserId, value.Value.UserId);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<UserResponse>
            {
                new UserResponse { UserId = 1, Email = "a@a.com", Username = "user1" },
                new UserResponse { UserId = 2, Email = "b@b.com", Username = "user2" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), default))
                .ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var value = Assert.IsType<ActionResult<List<UserResponse>>>(result);
            Assert.Equal(2, value.Value.Count);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNoContent()
        {
            // Arrange
            var userId = 1;
            var command = new UpdateUserCommand
            {
                Email = "updated@test.com",
                Username = "updateduser",
                Name = "Test",
            };

            var userResponse = new UserResponse
            {
                Email = "updated@test.com",
                Username = "updateduser",
                Name = "Test",
                Type = UserType.User
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<UpdateUserCommand>(c => c.Email == command.Email), default))
                .ReturnsAsync(userResponse);

            // Act
            var result = await _controller.UpdateAsync(userId, command);

            // Assert
            var value = Assert.IsType<ActionResult<UserResponse>>(result);
            Assert.Equal(userResponse.UserId, value.Value.UserId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNoContent()
        {
            // Arrange
            var userId = 1;

            _mediatorMock
                .Setup(m => m.Send(It.Is<DeleteUserCommand>(c => c.Id == userId), default))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.DeleteAsync(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetGamesByUserAsync_ShouldReturnUserGameLibrary()
        {
            // Arrange
            var expectedGames = new List<UserLibraryGameResponse>
            {
                 new UserLibraryGameResponse { GameId = 10, Name = "Game A", }
            };

            SetUserId(1);  

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetGamesByUserQuery>(q => q.Id == 1), default))
                .ReturnsAsync(expectedGames);

            // Act
            var result = await _controller.GetGamesByUserAsync();

            // Assert
            var value = Assert.IsType<ActionResult<List<UserLibraryGameResponse>>>(result);
            Assert.Equal(1, value.Value.Count);
            Assert.Equal("Game A", value.Value[0].Name);
        }


        [Fact]
        public async Task UpdateGameReleaseAsync_ShouldReturnNoContent()
        {
            // Arrange
            var userId = 1;
            var gameId = 10;

            var command = new ReleaseUserGameCommand
            {
                DateOfPurchase = DateTime.UtcNow,
                GameId = gameId,    
                UserId = userId,
            };

            SetUserId(userId); // mock GetUserId()

            _mediatorMock
                .Setup(m => m.Send(It.Is<ReleaseUserGameCommand>(c => c.GameId == gameId && c.UserId == userId), default))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.UpdateGameReleaseAsync(gameId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        private void SetUserId(int id)
        {
            _controller.ControllerContext.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim("id", id.ToString())
                })
            );
        }
    }
}
