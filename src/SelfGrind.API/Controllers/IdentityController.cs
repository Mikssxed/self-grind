using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfGrind.Application.User.Commands;
using SelfGrind.Application.User.Commands.RegisterUser;

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
}