using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Commands.CompleteTaskOccurence;

public class CompleteTaskOccurenceCommandHandler(ILogger<CompleteTaskOccurenceCommandHandler> logger, ITasksRepository tasksRepository, 
    IUserContext userContext) : IRequestHandler<CompleteTaskOccurenceCommand, Guid>
{
    public async Task<Guid> Handle(CompleteTaskOccurenceCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling CompleteTaskOccurenceCommand for TaskOccurrenceId: {taskOccurrenceId}", request.Id);
        
        var taskOccurence = await tasksRepository.GetTaskOccurenceById(currentUser.Id, request.Id, cancellationToken);

        if (taskOccurence == null)
        {
            throw new NotFoundException("task occurrence", request.Id.ToString());
        }

        taskOccurence.Status = TaskOccurrenceStatus.Done;
        taskOccurence.CompletedDate = DateOnly.FromDateTime(DateTime.UtcNow);

        await tasksRepository.CompleteOccurrenceAsync(cancellationToken);

        return taskOccurence.Id;
    }
}