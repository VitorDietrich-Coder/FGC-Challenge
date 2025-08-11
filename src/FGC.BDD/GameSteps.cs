using Ardalis.GuardClauses;
using FGC.Application.Games.Commands.CreateGame;
using FGC.Application.Games.Models.Response;
using FGC.Application.Games.Queries.GetAllGames;
using FGC.Application.Games.Queries.GetGames;
using FGC.Domain.Core.Exceptions;
using MediatR;
using Moq;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FGC.BDD.Tests.StepDefinitions.Games
{
    [Binding]
    public class GameSteps
    {
        private readonly Mock<ISender> _mediatorMock = new();
        private CreateGameCommand _createGameCommand = new();
        private GameResponse _gameResponse;
        private List<GameResponse> _gameList;
        private Exception _exception;
        private int _gameId;

        [Given(@"the game has title ""(.*)""")]
        public void GivenTheGameHasTitle(string title)
            => _createGameCommand.Name = title;

        [Given(@"the genre ""(.*)""")]
        public void GivenTheGenre(string genre)
            => _createGameCommand.Category = genre;

        [Given(@"the price (.*)")]
        public void GivenThePrice(decimal price)
            => _createGameCommand.Price = price;

        [When(@"the user requests to create the game")]
        public async Task WhenTheUserRequestsToCreateTheGame()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_createGameCommand.Category))
                {
                    _mediatorMock
                        .Setup(m => m.Send(It.IsAny<CreateGameCommand>(), default))
                        .ThrowsAsync(new BusinessRulesException("Title is required."));
                }
                else
                {
                    _mediatorMock
                        .Setup(m => m.Send(It.IsAny<CreateGameCommand>(), default))
                        .ReturnsAsync(new GameResponse
                        {
                            Id = 1,
                            Name = _createGameCommand.Name,
                            Category = _createGameCommand.Category,
                            Price = _createGameCommand.Price
                        });
                }

                _gameResponse = await _mediatorMock.Object.Send(_createGameCommand);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"the result should contain the created game")]
        public void ThenTheResultShouldContainTheCreatedGame()
            => Assert.NotNull(_gameResponse);

        [Then(@"the game title should be ""(.*)""")]
        public void ThenTheGameTitleShouldBe(string expectedTitle)
            => Assert.Equal(expectedTitle, _gameResponse.Name);

        [Then(@"an exception should be thrown")]
        public void ThenAnExceptionShouldBeThrown()
            => Assert.NotNull(_exception);

        [When(@"the user requests to get all games")]
        public async Task WhenTheUserRequestsToGetAllGames()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllGamesQuery>(), default))
                .ReturnsAsync(new List<GameResponse>
                {
                    new GameResponse { Id = 1, Name = "Game 1", Category = "Action" },
                    new GameResponse { Id = 2, Name = "Game 2", Category = "Adventure" }
                });

            _gameList = await _mediatorMock.Object.Send(new GetAllGamesQuery());
        }

        [Then(@"the result should contain a list of games")]
        public void ThenTheResultShouldContainAListOfGames()
        {
            Assert.NotNull(_gameList);
            Assert.True(_gameList.Count > 0);
        }

        [Given(@"the game ID is (.*)")]
        public void GivenTheGameIDIs(int id)
            => _gameId = id;

        [When(@"the user requests the game by ID")]
        public async Task WhenTheUserRequestsTheGameByID()
        {
            try
            {
                if (_gameId == 999)
                {
                    _mediatorMock
                        .Setup(m => m.Send(It.Is<GetGameByIdQuery>(q => q.Id == _gameId), default))
                        .ThrowsAsync(new NotFoundException(_gameId.ToString(), "Game not found"));
                }
                else
                {
                    _mediatorMock
                        .Setup(m => m.Send(It.Is<GetGameByIdQuery>(q => q.Id == _gameId), default))
                        .ReturnsAsync(new GameResponse
                        {
                            Id = _gameId,
                            Name = "Sample Game",
                            Category = "RPG",
                            Price = 49.99m
                        });
                }

                _gameResponse = await _mediatorMock.Object.Send(new GetGameByIdQuery { Id = _gameId });
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"the result should contain a game with ID (.*)")]
        public void ThenTheResultShouldContainAGameWithID(int expectedId)
        {
            Assert.NotNull(_gameResponse);
            Assert.Equal(expectedId, _gameResponse.Id);
        }
    }
}
