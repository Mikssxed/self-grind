using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace SelfGrind.Application.User.Commands.RegisterUser;

public class RegisterUserCommandHandler(
    ILogger<RegisterUserCommandHandler> logger,
    IHttpContextAccessor httpContextAccessor,
    IUserStore<Domain.Entities.User> userStore,
    UserManager<Domain.Entities.User> userManager,
    LinkGenerator linkGenerator,
    IEmailSender<Domain.Entities.User> emailSender)
    : IRequestHandler<RegisterUserCommand, Results<Ok, ValidationProblem>>
{
    private static readonly EmailAddressAttribute _emailAddressAttribute = new();
    
    
    public async Task<Results<Ok, ValidationProblem>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailStore = (IUserEmailStore<Domain.Entities.User>)userStore;
        var email = request.Email;
        var username = request.Username;
        
        if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
        {
            return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
        }
        
        var user = new Domain.Entities.User();
        await userStore.SetUserNameAsync(user, username, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, request.Password);
        
        if (!result.Succeeded)
        {
            return CreateValidationProblem(result);
        }
        
        // await SendConfirmationEmailAsync(user, email);
        return TypedResults.Ok();
    }

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(!result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }
    
    private async Task SendConfirmationEmailAsync(Domain.Entities.User user, string email, bool isChange = false)
    {
        var code = isChange
            ? await userManager.GenerateChangeEmailTokenAsync(user, email)
            : await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var userId = await userManager.GetUserIdAsync(user);
        var routeValues = new RouteValueDictionary()
        {
            ["userId"] = userId,
            ["code"] = code,
        };

        if (isChange)
        {
            // This is validated by the /confirmEmail endpoint on change.
            routeValues.Add("changedEmail", email);
        }

        var confirmEmailEndpointName = "MapIdentityApi-/confirmEmail";
        var httpContext = httpContextAccessor.HttpContext;
        var confirmEmailUrl = linkGenerator.GetUriByName(httpContext, confirmEmailEndpointName, routeValues)
                              ?? throw new NotSupportedException($"Endpoint with name not found '{confirmEmailEndpointName}'.");

        await emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
    }
}