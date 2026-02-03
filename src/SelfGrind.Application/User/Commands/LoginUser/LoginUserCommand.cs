using MediatR;

namespace SelfGrind.Application.User.Commands.LoginUser;

public class LoginUserCommand : IRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}