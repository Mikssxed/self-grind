using MediatR;
using SelfGrind.Application.User.Commands.LoginUser;

namespace SelfGrind.Application.User.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<LoginResponse>
{
    public string RefreshToken { get; set; } = string.Empty;
}
