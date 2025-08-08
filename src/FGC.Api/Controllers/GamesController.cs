using FGC.Api.Swagger.Attributes;
using FGC.Application.Games.Commands.CreateGame;
using FGC.Application.Games.Models.Response;
using FGC.Application.Games.Queries.GetAllGames;
using FGC.Application.Games.Queries.GetGames;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FGC.Api.Controllers
{
    /// <summary>
    /// Manages game-related operations such as creation, retrieval, and listing.
    /// </summary>
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GamesController : ApiControllerBase
    {
        /// <summary>
        /// Creates a new game.
        /// </summary>
        /// <param name="request">Game details to be created.</param>
        /// <returns>The created game data.</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create game",
            Description = "Registers a new game with details such as title, genre, price, and optional promotion."
        )]
        [SwaggerResponseProfile("Games.Create")]
        public async Task<ActionResult<GameResponse>> Create([FromBody] CreateGameCommand command)
        {
           return await Mediator.Send(command);
        }

        /// <summary>
        /// Lists all games.
        /// </summary>
        /// <returns>A list of all games.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = "List all games",
            Description = "Returns a collection of all games available in the system."
        )]

        [SwaggerResponseProfile("Games.GetAll")]
        public async Task<ActionResult<List<GameResponse>>> GetAll()
        {
            return await Mediator.Send(new GetAllGamesQuery());
        }

        /// <summary>
        /// Gets a game by its ID.
        /// </summary>
        /// <param name="id">Game ID.</param>
        /// <returns>Game details if found.</returns>
        [HttpGet("{id:int:min(1)}")]
        [SwaggerOperation(
            Summary = "Get Games by ID",
            Description = "Returns game details for the given ID. Returns 404 if the game doesn't exist."
        )]

        [SwaggerResponseProfile("Games.Get")]
        public async Task<ActionResult<GameResponse>> GetAsync(int id)
        {
            return await Mediator.Send(new GetGameByIdQuery {Id  = id } );

        }
    }
}
