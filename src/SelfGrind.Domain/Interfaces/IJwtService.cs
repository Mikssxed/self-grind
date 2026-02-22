using SelfGrind.Domain.Entities;

namespace SelfGrind.Domain.Interfaces;

public interface IJwtService
{
    Task<(string accessToken, string refreshToken, int expiresIn)> GenerateTokensAsync(User user);
    Task<string> GenerateAccessTokenAsync(User user);
    Task<string> GenerateRefreshTokenAsync();
}
