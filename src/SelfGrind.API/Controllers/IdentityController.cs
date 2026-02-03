using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfGrind.Application.User.Commands;
using SelfGrind.Application.User.Commands.RegisterUser;
using SelfGrind.Application.User.Commands.ConfirmUser;
using SelfGrind.Application.User.Commands.LoginUser;

namespace SelfGrind.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPatch("user")]
    [Authorize]
    public async Task<IActionResult> UpdateUserDetails([FromBody]UpdateUserDetailsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody]RegisterUserCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    
    [HttpPost("confirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmEmailCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody]LoginUserCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}