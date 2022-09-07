using Onion.Contracts.Identity;

namespace Onion.Api.Routes.Identity;

public class GrantRoleEndpoint : IRouteEndpoint
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(Uri.Identity.GrantRole, GrantRole)
           .WithTags(Uri.Identity.Tag);
    }

    internal async Task<IResult> GrantRole([FromServices] ISender sender,
        GrantRoleRequest request, HttpContext httpContext)
    {
        var result = await sender.Send(request.AsCommand());

        return result.Match(
            value => Results.Ok(value),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to grant role.",
            },
            httpContext));
    }
}
