using MediatR;
using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommand : IRequest<Guid>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int Exp { get; set; }
    public BaseAttribute Attribute { get; set; }
    
    
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TaskRepetitionType RepetitionType { get; set; }
    public int RepeatInterval { get; set; } = 1;
    public List<DayOfWeek>? DaysOfWeek { get; set; } = [];
}