namespace Onion.Api.Routes.Common;

public class RootEndpoint : IRouteEndpoint
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/", () => Results.Ok()).WithTags("/");
    }
}
