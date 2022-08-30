using Mapster;
using Onion.Application.Identity.Commands.Registration;
using Onion.Contracts.Identity;

namespace Onion.Api.Routes.Identity;

public class RegistrationEndpoint : IRouteEndpoint
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(Uri.Identity.Register, Register);
    }

    internal async Task<IResult> Register([FromServices] ISender sender,
        RegisterRequest request, HttpContext httpContext)
    {
        var command = request.Adapt<RegisterCommand>();
        var result = await sender.Send(command);
        return result.HttpResult(httpContext, new ProblemDetails
        {
            Title = "Registration failed.",
        });
    }
}
