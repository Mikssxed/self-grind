using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SelfGrind.Application.Behaviors;
using SelfGrind.Application.Character.Services;
using SelfGrind.Application.Settings;
using SelfGrind.Application.Stats.Services;
using SelfGrind.Application.User;

namespace SelfGrind.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<StatsSettings>()
            .BindConfiguration("StatsSettings")
            .ValidateDataAnnotations()
            .ValidateOnStart();
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IStatsService, StatsService>();
        services.AddScoped<IHeroStatCalculator, HeroStatCalculator>();
        services.AddScoped<IEvolutionTierResolver, EvolutionTierResolver>();
        services.AddScoped<IItemGrantingService, ItemGrantingService>();
        services.AddScoped<IUserItemGranter, UserItemGranter>();
        services.AddHttpContextAccessor();
    }
}