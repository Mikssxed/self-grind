using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfGrind.Application.Tasks.Commands.CreateTask;

namespace SelfGrind.Controllers;
[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous] // @todo Remove this line when authentication is implemented
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {
        await mediator.Send(command);
        
        return NoContent();
    }
}