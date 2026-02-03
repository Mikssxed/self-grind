using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace SelfGrind.Application.User.Commands.LoginUser;

public class LoginUserCommandHandler(
    ILogger<LoginUserCommandHandler> logger,
    SignInManager<Domain.Entities.User> signInManager
    )
    : IRequestHandler<LoginUserCommand>
{
    public Task Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Logging in user {username}", request.Username);

        var result = signInManager.PasswordSignInAsync(request.Username, request.Password, isPersistent: false, lockoutOnFailure: false);

        return result.ToString();
    }
}