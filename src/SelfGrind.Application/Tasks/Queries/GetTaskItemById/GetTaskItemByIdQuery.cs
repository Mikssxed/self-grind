using MediatR;
using SelfGrind.Application.Tasks.Dtos;

namespace SelfGrind.Application.Tasks.Queries.GetTaskItemById;

public class GetTaskItemByIdQuery(Guid id) : IRequest<TaskItemDto>
{
    public Guid Id { get; } = id;
}