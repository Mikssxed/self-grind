using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfGrind.Application.Habits.Commands.CreateHabit;

namespace SelfGrind.Controllers;

[ApiController]
[Route("api/habits")]
[Authorize]
public class HabitsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> CreateHabit([FromBody] CreateHabitCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}