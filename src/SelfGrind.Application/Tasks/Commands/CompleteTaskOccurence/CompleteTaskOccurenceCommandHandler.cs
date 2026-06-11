using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Character.Services;
using SelfGrind.Application.Common;
using SelfGrind.Application.Stats.Services;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Commands.CompleteTaskOccurence;

public class CompleteTaskOccurenceCommandHandler(
    ILogger<CompleteTaskOccurenceCommandHandler> logger,
    ITasksRepository tasksRepository,
    IUsersRepository usersRepository,
    IUserItemGranter userItemGranter,
    IStatsService statsService,
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

        if (taskOccurence.Status == TaskOccurrenceStatus.Done)
        {
            return taskOccurence.Id;
        }

        taskOccurence.Status = TaskOccurrenceStatus.Done;
        taskOccurence.CompletedDate = DateUtils.UtcToday;

        var user = await usersRepository.GetWithStatsOrThrowAsync(currentUser.Id, cancellationToken);

        var levelledUp = statsService.AwardTaskExp(user, taskOccurence.TaskItem.Attribute, taskOccurence.TaskItem.Exp);

        await tasksRepository.SaveChangesAsync(cancellationToken);

        if (levelledUp)
        {
            await userItemGranter.GrantUnlockedItemsAsync(user.Id, user.Level, cancellationToken);
        }

        return taskOccurence.Id;
    }
}
