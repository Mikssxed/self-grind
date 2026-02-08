using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SelfGrind.Domain.Exceptions;

namespace SelfGrind.Application.User.Commands.LoginUser;

public class LoginUserCommandHandler(
    ILogger<LoginUserCommandHandler> logger,
    SignInManager<Domain.Entities.User> signInManager
    )
    : IRequestHandler<LoginUserCommand>
{
    public async Task Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Logging in user {username}", request.Email);
        
        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);
        
        if (user == null || user.UserName == null)
        {
            throw new NotFoundException("user", request.Email);
        }
        
        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        
        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;
        await signInManager.SignInAsync(user, true);
    }
}