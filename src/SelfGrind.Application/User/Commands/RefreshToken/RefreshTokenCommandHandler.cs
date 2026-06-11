using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User.Commands.LoginUser;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Interfaces;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.User.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    ILogger<RefreshTokenCommandHandler> logger,
    IRefreshTokensRepository refreshTokensRepository,
    IJwtService jwtService)
    : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = jwtService.HashRefreshToken(request.RefreshToken);
        var storedToken = await refreshTokensRepository.GetByHashAsync(tokenHash, cancellationToken);

        if (storedToken is null || !storedToken.IsActive)
        {
            throw new AuthenticationException("Invalid refresh token.");
        }

        logger.LogInformation("Rotating refresh token for user {UserId}", storedToken.UserId);

        // Rotation: the presented token is single-use
        storedToken.RevokedAt = DateTime.UtcNow;

        var (accessToken, refreshToken, expiresIn) = await jwtService.GenerateTokensAsync(storedToken.User);

        await refreshTokensRepository.AddAsync(new Domain.Entities.RefreshToken
        {
            UserId = storedToken.UserId,
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
