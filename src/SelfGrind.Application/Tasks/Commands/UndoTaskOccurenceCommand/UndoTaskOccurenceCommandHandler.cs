using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Stats.Services;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Commands.UndoTaskOccurenceCommand;

public class UndoTaskOccurenceCommandHandler(
    ILogger<UndoTaskOccurenceCommandHandler> logger,
    ITasksRepository tasksRepository,
    IUsersRepository usersRepository,
    IStatsService statsService,
    IUserContext userContext) : IRequestHandler<UndoTaskOccurenceCommand, Guid>
{
    public async Task<Guid> Handle(UndoTaskOccurenceCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling UndoTaskOccurenceCommand for TaskOccurrenceId: {taskOccurrenceId}", request.Id);

        var taskOccurence = await tasksRepository.GetTaskOccurenceById(currentUser.Id, request.Id, cancellationToken);

        if (taskOccurence == null)
        {
            throw new NotFoundException("task occurrence", request.Id.ToString());
        }

        if (taskOccurence.Status != TaskOccurrenceStatus.Done)
        {
            return taskOccurence.Id;
        }

        taskOccurence.Status = TaskOccurrenceStatus.Pending;
        taskOccurence.CompletedDate = null;

        var user = await usersRepository.GetWithStatsAsync(currentUser.Id, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("user", currentUser.Id);
        }

        var exp = taskOccurence.TaskItem.Exp;
        statsService.RevokeUserExp(user, exp);

        var stat = user.Stats.FirstOrDefault(s => s.Attribute == taskOccurence.TaskItem.Attribute);
        if (stat != null)
        {
            statsService.RevokeStatExp(stat, exp);
        }

        await tasksRepository.SaveChangesAsync(cancellationToken);

        return taskOccurence.Id;
    }
}
