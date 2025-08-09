using FGC.Application.Games.Commands.CreateGame;
using FGC.Application.Games.Models.Response;
using FGC.Application.Games.Queries.GetAllGames;
using FGC.Application.Games.Queries.GetGames;
using FGC.Application.UnitTests;
using FGC.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FGC.Api.UnitTests.Login
{
    public class GamesControllerTests : TestFixture
    {
        private readonly GamesController _controller;
        private readonly Mock<ISender> _mediatorMock;

        public GamesControllerTests()
        {
            _mediatorMock = new Mock<ISender>();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(_mediatorMock.Object)
                .BuildServiceProvider();

            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            _controller = new GamesController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task GetAsync_ShouldReturnGame_WhenIdIsValid()
        {
            // Arrange
            var gameId = 1;
            var expectedGame = new GameResponse
            {
                Id = gameId,
                Name = "Test Game",
                Category = "Action"
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetGameByIdQuery>(q => q.Id == gameId), default))
                .ReturnsAsync(expectedGame);

            // Act
            var result = await _controller.GetAsync(gameId);

            // Assert
            var value = Assert.IsType<ActionResult<GameResponse>>(result);
            Assert.Equal(expectedGame.Id, value.Value.Id);
            Assert.Equal(expectedGame.Name, value.Value.Name);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfGames()
        {
            // Arrange
            var expectedGames = new List<GameResponse>
            {
                new GameResponse { Id = 1, Name = "Game 1", Category = "Adventure" },
                new GameResponse { Id = 2, Name = "Game 2", Category = "RPG" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllGamesQuery>(), default))
                .ReturnsAsync(expectedGames);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var value = Assert.IsType<ActionResult<List<GameResponse>>>(result);
            Assert.Equal(2, value.Value.Count);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedGame()
        {
            // Arrange
            var command = new CreateGameCommand
            {
                Name = "New Game",
                Category = "Strategy",
                Price = 59.99m
            };

            var expectedResponse = new GameResponse
            {
                Id = 99,
                Name = command.Name,
                Category = command.Category,
                Price = command.Price
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<CreateGameCommand>(c => c.Name == command.Name), default))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(command);

            // Assert
            var value = Assert.IsType<ActionResult<GameResponse>>(result);
            Assert.Equal(expectedResponse.Id, value.Value.Id);
            Assert.Equal(expectedResponse.Name, value.Value.Name);
        }
    }
}
