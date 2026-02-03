using MediatR;

namespace SelfGrind.Application.User.Commands.ConfirmUser;

public class ConfirmEmailCommand : IRequest
{
    public required string UserId { get; set; }
    public required string ConfirmationCode { get; set; }
}