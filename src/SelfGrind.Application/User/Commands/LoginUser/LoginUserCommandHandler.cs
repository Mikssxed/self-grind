using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Interfaces;

namespace SelfGrind.Application.User.Commands.LoginUser;

public class LoginUserCommandHandler(
    ILogger<LoginUserCommandHandler> logger,
    SignInManager<Domain.Entities.User> signInManager,
    IJwtService jwtService
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
            throw new AuthenticationException("Invalid email or password.");
        }

        // Generate JWT tokens
        var (accessToken, refreshToken, expiresIn) = await jwtService.GenerateTokensAsync(user);

        return new LoginResponse
        {
            TokenType = "Bearer",
            AccessToken = accessToken,
            ExpiresIn = expiresIn,
            RefreshToken = refreshToken
        };
    }
}