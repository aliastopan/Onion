using Mapster;
using Onion.Application.Identity.Commands.Authentication;
using Onion.Contracts.Identity;

namespace Onion.Api.Routes.Identity;

public class AuthenticationEndpoint : IRouteEndpoint
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(Uri.Identity.Login, Login);
    }

    internal async Task<IResult> Login([FromServices] ISender sender,
        LoginRequest request, HttpContext httpContext)
    {
        var command = request.Adapt<LoginCommand>();
        var result = await sender.Send(command);
        return result.HttpResult(httpContext, new ProblemDetails
        {
            Title = "Login failed.",
        });
    }
}
