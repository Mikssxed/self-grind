using MediatR;

namespace SelfGrind.Application.User.Commands;

public class UpdateUserDetailsCommand : IRequest
{
    public string Username { get; set; }
}