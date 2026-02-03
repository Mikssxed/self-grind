using Microsoft.AspNetCore.Identity;
using SelfGrind.Domain.Exceptions;

namespace SelfGrind.Application.Behaviors;

public static class IdentityResultExtensions
{
    private static readonly string[] UsernameErrors = { "DuplicateUserName", "InvalidUserName" };
    private static readonly string[] EmailErrors = { "DuplicateEmail", "InvalidEmail" };
    private static readonly string[] PasswordErrors = { "Password" }; // All errors starting with "Password"
    private static readonly string[] RoleErrors = { "UserAlreadyInRole", "UserNotInRole", "DuplicateRoleName", "InvalidRoleName" };
    private static readonly string[] TokenErrors = { "InvalidToken" };
    private static readonly string[] RecoveryErrors = { "RecoveryCodeRedemptionFailed" };

    public static void ThrowIfFailed(this IdentityResult result)
    {
        if (!result.Succeeded)
        {
            var errors = new Dictionary<string, string[]>();

            foreach (var error in result.Errors)
            {
                var fieldName = MapErrorCodeToFieldName(error.Code);

                if (!errors.ContainsKey(fieldName))
                {
                    errors[fieldName] = new[] { error.Description };
                }
                else
                {
                    errors[fieldName] = errors[fieldName].Append(error.Description).ToArray();
                }
            }

            throw new ValidationException(errors);
        }
    }

    private static string MapErrorCodeToFieldName(string errorCode)
    {
        if (UsernameErrors.Contains(errorCode)) return "username";
        if (EmailErrors.Contains(errorCode)) return "email";
        if (RoleErrors.Contains(errorCode)) return "role";
        if (TokenErrors.Contains(errorCode)) return "token";
        if (RecoveryErrors.Contains(errorCode)) return "recoveryCode";

        if (errorCode.StartsWith("Password", StringComparison.OrdinalIgnoreCase)) return "password";

        return "general";
    }
}
