using FGC.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using FGC.Application.Auth.Models.Response;
using Swashbuckle.AspNetCore.Filters;
using FGC.Application.Auth.Commands;
using Microsoft.AspNetCore.Authorization;
using FGC.Api.Swagger.Attributes;

namespace FGC.Api.Controllers
{
    /// <summary>
    /// Handles user authentication and token generation.
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ApiControllerBase
    {
        /// <summary>
        /// Authenticates the user with provided credentials and returns a JWT token on success.
        /// </summary>
        /// <param name="request">User login data (username and password).</param>
        /// <returns>JWT token if login is successful; error details otherwise.</returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [SwaggerOperation(
            Summary = "User login and JWT token generation.",
            Description = "Validates user credentials. Returns a JWT token to authorize subsequent requests if successful. " +
                          "Returns 400 for invalid input or 404 if the user is not found."
        )]

        [SwaggerResponseProfile("Auth")]
        public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
