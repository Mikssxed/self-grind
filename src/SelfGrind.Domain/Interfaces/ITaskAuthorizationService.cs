using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Interfaces;

public interface ITaskAuthorizationService
{
    bool Authorize(TaskItem task, ResourceOperation operation);
}