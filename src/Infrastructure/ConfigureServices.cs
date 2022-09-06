using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Onion.Infrastructure.Authentication;

namespace Onion.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ISecureHash, SecureHashProvider>();
        services.AddInfrastructureDbContext(configuration);
        services.AddScoped<IJwtService, JwtProvider>();
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
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
