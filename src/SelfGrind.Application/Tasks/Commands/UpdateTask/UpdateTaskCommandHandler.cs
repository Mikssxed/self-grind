using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Interfaces;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler(
    ILogger<UpdateTaskCommandHandler> logger,
    IMapper mapper,
    ITasksRepository tasksRepository,
    IUserContext userContext,
    ITaskAuthorizationService authorizationService)
    : IRequestHandler<UpdateTaskCommand>
{
    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Updating task {TaskId} for user {UserId}", request.Id, currentUser.Id);

        var task = await tasksRepository.GetByIdAsync(currentUser.Id, request.Id, cancellationToken);
        if (task is null)
            throw new NotFoundException("task", request.Id.ToString());

        if (!authorizationService.Authorize(task, ResourceOperation.Update))
            throw new ForbidException();

        mapper.Map(request, task);

        task.Schedule.StartDate = request.StartDate;
        task.Schedule.EndDate = request.EndDate;
        task.Schedule.RepetitionType = request.RepetitionType;
        task.Schedule.RepeatInterval = request.RepeatInterval;
        task.Schedule.DaysOfWeek = request.DaysOfWeek;

        await tasksRepository.UpdateAsync(task, cancellationToken);
    }
}
