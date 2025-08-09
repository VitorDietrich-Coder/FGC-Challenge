using FGC.Api.Swagger.Attributes;
using FGC.Application.Common;
using FGC.Application.Users.Commands.CreateUser;
using FGC.Application.Users.Commands.DeleteUser;
using FGC.Application.Users.Commands.UpdateUser;
using FGC.Application.Users.Commands.UpdateUser.ReleaseUserGame;
using FGC.Application.Users.Models.Response;
using FGC.Application.Users.Queries.GetAll;
using FGC.Application.Users.Queries.GetUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace FGC.Api.Controllers
{
    /// <summary>
    /// Manages operations related to system users.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ApiControllerBase
    {
        /// <summary>
        /// Creates a new regular user.
        /// </summary>
        [Authorize(Roles = "User,Admin")]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Creates a new regular user.",
            Description = "Registers a user with basic permissions. Returns the user data or an error if validation fails or the email is already taken."
        )]
        [SwaggerResponseProfile("User.Register")]
        [ProducesResponseType(typeof(SuccessResponse<UserResponse>), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<UserResponse>> CreateAsync([FromBody] CreateUserCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Creates a new user with assigned privileges (admin only).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost("create-admin")]
        [SwaggerOperation(
            Summary = "Creates a new user with assigned privileges (admin only).",
            Description = "Allows creation of users with admin or regular roles. Requires administrator privileges."
        )]
        [SwaggerResponseProfile("Admin.Register")]
        [ProducesResponseType(typeof(SuccessResponse<UserResponse>), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<UserResponse>> CreateAdminAsync([FromBody] CreateUserCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        [HttpPut("{id:int:min(1)}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Updates an existing user's information.",
            Description = "Modifies user details such as name, email, or status. Only accessible by administrators."
        )]
        [SwaggerResponseProfile("User.Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<UserResponse>> UpdateAsync(int id, [FromBody] UpdateUserCommand command)
        {
           command.Id = id;
           return await Mediator.Send(command);
        }


        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        [HttpDelete("{id:int:min(1)}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Deletes a user by ID.",
            Description = "Deletes the user identified by the given ID. Only accessible by administrators."
        )]
        [SwaggerResponseProfile("User.Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await Mediator.Send(new DeleteUserCommand { Id = id });

            return NoContent();
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        [HttpGet("{id:int:min(1)}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Retrieves a user by ID.",
            Description = "Returns the user data based on the provided ID. Requires administrator access."
        )]
        [SwaggerResponseProfile("User.Get")]
        public async Task<ActionResult<UserResponse>> GetAsync(int id)
        {
            return await Mediator.Send(new GetUserQuery { Id = id });
        }

        /// <summary>
        /// Retrieves all registered users.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Retrieves all users.",
            Description = "Returns a list of all users in the system. Access restricted to administrators."
        )]
        [SwaggerResponseProfile("User.GetAll")]
        public async Task<ActionResult<List<UserResponse>>> GetAllAsync()
        {
            return await Mediator.Send(new GetAllUsersQuery());
        }

        /// <summary>
        /// Retrieves the authenticated users game library.
        /// </summary>
        [HttpGet("users-games")]
        [Authorize(Roles = "User,Admin")]
        [SwaggerOperation(
            Summary = "Retrieves the authenticated user game library.",
            Description = "Retrieves the list of games linked to the currently authenticated user. Accessible by both regular users and administrators."
        )]
        [SwaggerResponseProfile("User.Get.UsersGames")]
        public async Task<ActionResult<List<UserLibraryGameResponse>>> GetGamesByUserAsync()
        {
            var id = GetUserId();
            return await Mediator.Send(new GetGamesByUserQuery {Id = id});
        }

        /// <summary>
        /// Updates the release status of a game in a user's library.
        /// </summary>
        [HttpPatch("{userId:int:min(1)}/games/{gameId:int:min(1)}/release")]
        [Authorize(Roles = "Admin,User")]
        [SwaggerOperation(
            Summary = "Updates the release status of a game in a user's library.",
            Description = "Allows updating the release status of a specific game owned by the user. Accessible by the user or an administrator."
        )]
        [SwaggerResponseProfile("User.Game.ReleaseUpdate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateGameReleaseAsync(int userId, int gameId, [FromBody] ReleaseUserGameCommand command)
        {
            command.UserId = GetUserId();
            command.GameId = gameId;

            await Mediator.Send(command);
            return NoContent();
        }
    }
}
