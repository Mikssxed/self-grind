using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Interfaces;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.User.Commands.LoginUser;

public class LoginUserCommandHandler(
    ILogger<LoginUserCommandHandler> logger,
    SignInManager<Domain.Entities.User> signInManager,
    IRefreshTokensRepository refreshTokensRepository,
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
            // Same exception as a wrong password so responses don't reveal whether the email exists
            throw new AuthenticationException("Invalid email or password.");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            throw new AuthenticationException("Invalid email or password.");
        }

        // Generate JWT tokens
        var (accessToken, refreshToken, expiresIn) = await jwtService.GenerateTokensAsync(user);

        await refreshTokensRepository.AddAsync(new Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            TokenHash = jwtService.HashRefreshToken(refreshToken),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = jwtService.GetRefreshTokenExpiry(),
        }, cancellationToken);

        return new LoginResponse
        {
            TokenType = "Bearer",
            AccessToken = accessToken,
            ExpiresIn = expiresIn,
            RefreshToken = refreshToken
        };
    }
}