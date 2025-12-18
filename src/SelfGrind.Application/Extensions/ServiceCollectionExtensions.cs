using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SelfGrind.Application.Behaviors;
using SelfGrind.Application.User;

namespace SelfGrind.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);
        });
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();
    }
}