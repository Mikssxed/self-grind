using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SelfGrind.Infrastructure.Persistence;

namespace SelfGrind.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SelfGrindDb");
        services.AddDbContext<SelfGrindDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging());
    }
}