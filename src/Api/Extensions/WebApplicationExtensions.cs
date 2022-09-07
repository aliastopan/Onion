using Onion.Application.Common.Interfaces;

namespace Onion.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void Init(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<IInitializerService>().Seed();
    }
}
