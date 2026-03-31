using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfGrind.Application.Tasks.Commands.CompleteTaskOccurence;

namespace SelfGrind.Controllers;

[ApiController]
[Route("api/occurrences")]
[Authorize]
public class TaskOccurrencesController(IMediator mediator) : ControllerBase
{
    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CompleteOccurrence([FromRoute] Guid id)
    {
        await mediator.Send(new CompleteTaskOccurenceCommand(id));
        return NoContent();
    }
}
