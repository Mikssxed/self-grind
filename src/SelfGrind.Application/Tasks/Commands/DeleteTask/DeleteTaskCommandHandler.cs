using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Interfaces;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler(
    ILogger<DeleteTaskCommandHandler> logger,
    ITasksRepository tasksRepository,
    IUserContext userContext,
    ITaskAuthorizationService authorizationService)
    : IRequestHandler<DeleteTaskCommand>
{
    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Archiving task {TaskId} for user {UserId}", request.Id, currentUser.Id);

        var task = await tasksRepository.GetByIdAsync(currentUser.Id, request.Id, cancellationToken);
        if (task is null)
            throw new NotFoundException("task", request.Id.ToString());

        if (!authorizationService.Authorize(task, ResourceOperation.Delete))
            throw new ForbidException();

        await tasksRepository.ArchiveAsync(task, cancellationToken);
    }
}
