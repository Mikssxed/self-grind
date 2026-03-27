using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfGrind.Application.Tasks.Commands.CreateTask;
using SelfGrind.Application.Tasks.Dtos;
using SelfGrind.Application.Tasks.Queries;

namespace SelfGrind.Controllers;
[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {
        await mediator.Send(command);
        
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskItemDto[]>> GetAllTasks()
    {
        var tasks = await mediator.Send(new GetAllTaskItemsQuery());
        return Ok(tasks);
    }
}