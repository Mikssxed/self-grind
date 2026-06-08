using MediatR;
using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Tasks.Commands.LogActivity;

public class LogActivityCommand : IRequest<Guid>
{
    public required string Title { get; set; }
    public string? Notes { get; set; }
    public int Exp { get; set; }
    public BaseAttribute Attribute { get; set; }
}
