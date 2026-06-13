using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SelfGrind.Application.User.Commands;
using SelfGrind.Application.User.Commands.RegisterUser;
using SelfGrind.Application.User.Commands.ConfirmUser;
using SelfGrind.Application.User.Commands.LoginUser;
using SelfGrind.Application.User.Commands.RefreshToken;
using SelfGrind.Application.User.Dtos;
using SelfGrind.Application.User.Queries.GetCurrentUser;
using SelfGrind.Models;

namespace SelfGrind.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpGet("user")]
    [Authorize]
    [ProducesResponseType(typeof(CurrentUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CurrentUserDto>> GetCurrentUser()
    {
        var currentUser = await mediator.Send(new GetCurrentUserQuery());
        return Ok(currentUser);
    }

    [HttpPatch("user")]
    [Authorize]
    public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserDetailsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("register")]
    [EnableRateLimiting(Extensions.WebApplicationBuilderExtensions.AuthRateLimitPolicy)]
    [ProducesResponseType(typeof(ApiOperationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiOperationResult), StatusCodes.Status400BadRequest)]
    public async Task<Ok<ApiOperationResult>> RegisterUser([FromBody] RegisterUserCommand command)
    {
        await mediator.Send(command);
        return ApiOperationResult.Ok();
    }

    [HttpPost("confirmEmail")]
    [EnableRateLimiting(Extensions.WebApplicationBuilderExtensions.AuthRateLimitPolicy)]
    [ProducesResponseType(typeof(ApiOperationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiOperationResult), StatusCodes.Status400BadRequest)]
    public async Task<Ok<ApiOperationResult>> ConfirmEmail([FromBody] ConfirmEmailCommand command)
    {
        await mediator.Send(command);
        return ApiOperationResult.Ok();
    }

    [HttpPost("login")]
    [EnableRateLimiting(Extensions.WebApplicationBuilderExtensions.AuthRateLimitPolicy)]
    [ProducesResponseType(typeof(ApiOperationResult<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiOperationResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiOperationResult), StatusCodes.Status401Unauthorized)]
    public async Task<Ok<ApiOperationResult<LoginResponse>>> LoginUser([FromBody] LoginUserCommand command)
    {
        var response = await mediator.Send(command);
        return ApiOperationResult.Ok(response);
    }

    [HttpPost("refresh")]
    [EnableRateLimiting(Extensions.WebApplicationBuilderExtensions.AuthRateLimitPolicy)]
    [ProducesResponseType(typeof(ApiOperationResult<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiOperationResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiOperationResult), StatusCodes.Status401Unauthorized)]
    public async Task<Ok<ApiOperationResult<LoginResponse>>> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var response = await mediator.Send(command);
        return ApiOperationResult.Ok(response);
    }
}