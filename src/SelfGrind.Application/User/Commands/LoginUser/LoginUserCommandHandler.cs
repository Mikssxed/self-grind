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
    : IRequestHandler<LoginUserCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
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

        // For now, return empty response - we'll implement JWT generation next
        // The built-in Identity API endpoints handle token generation automatically
        // but since we want custom control, we need to implement our own JWT service
        return new LoginResponse
        {
            TokenType = "Bearer",
            AccessToken = "temporary_token", // TODO: Implement JWT generation
            ExpiresIn = 3600,
            RefreshToken = "temporary_refresh_token"
        };
    }
}