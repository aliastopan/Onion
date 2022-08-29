using System.Reflection;

namespace Onion.Api.Extensions;

internal static class RouteExtensions
{
    internal static IServiceCollection AddRouteEndpoints(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var routeEndpoints = new List<IRouteEndpoint>();
        foreach(var assembly in assemblies)
        {
            routeEndpoints.AddRange(assembly.ExportedTypes
                .Where(x => typeof(IRouteEndpoint).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IRouteEndpoint>()
            );
        }

        routeEndpoints.ForEach(service =>
        {
            if(service is IRouteService routeService)
            {
                routeService.DefineServices(services);
            }
        });
        services.AddSingleton(routeEndpoints as IReadOnlyCollection<IRouteEndpoint>);
        return services;
    }

    internal static WebApplication UseRouteEndpoints(this WebApplication app)
    {
        var routeEndpoints = app.Services.GetRequiredService<IReadOnlyCollection<IRouteEndpoint>>();
        foreach(var route in routeEndpoints)
        {
            route.DefineEndpoints(app);
        }
        return app;
    }
}
