using Microsoft.Extensions.Logging;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Interfaces;

namespace SelfGrind.Infrastructure.Services;

public class TaskAuthorizationService(ILogger<TaskAuthorizationService> logger, IUserContext userContext) : ITaskAuthorizationService
{
    public bool Authorize(TaskItem task, ResourceOperation operation)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Authorizing user: {@UserId} for {@Operation} on task: {@TaskId}", user?.Id, operation, task.Id);
        
        if (operation == ResourceOperation.Read || operation == ResourceOperation.Create)
        {
            logger.LogInformation("Create/read operation - authorization granted");
            return true;
        }

        if (operation == ResourceOperation.Delete && user.IsInRole(nameof(UserRoles.Admin)))
        {
            logger.LogInformation("Admin user, delete operation - authorization granted");
            return true;
        }

        if (operation == ResourceOperation.Delete || (operation == ResourceOperation.Update && user.Id == task.UserId))
        {
            logger.LogInformation("Owner user, update/delete operation - authorization granted");
            return true;
        }

        return false;
    }
}