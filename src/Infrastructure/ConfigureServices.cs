using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Onion.Application.Common.Interfaces;
using Onion.Infrastructure.Extensions;
using Onion.Infrastructure.Persistence;

namespace Onion.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureDbContext(configuration);
        return services;
    }

    internal static IServiceCollection AddInfrastructureDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        if(configuration.UseInMemoryDatabase())
        {
            services.AddDbContext<IDbContext, ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase(nameof(ApplicationDbContext));
            });
        }
        else
        {
            throw new NotImplementedException();
        }
        return services;
    }
}
