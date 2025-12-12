using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Interfaces;
using SelfGrind.Domain.Repositories;
using SelfGrind.Infrastructure.Persistence;
using SelfGrind.Infrastructure.Repositories;
using SelfGrind.Infrastructure.Services;

namespace SelfGrind.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SelfGrindDb");
        services.AddDbContext<SelfGrindDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging());
        
        services.AddIdentityApiEndpoints<User>().AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<SelfGrindDbContext>();
        
        services.AddScoped<ITasksRepository, TasksRepository>();

        services.AddScoped<ITaskAuthorizationService, TaskAuthorizationService>();
    }
}