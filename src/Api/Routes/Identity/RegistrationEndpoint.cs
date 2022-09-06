using Onion.Contracts.Identity;

namespace Onion.Api.Routes.Identity;

public class RegistrationEndpoint : IRouteEndpoint
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(Uri.Identity.Register, Register)
           .AllowAnonymous();
    }

    internal async Task<IResult> Register([FromServices] ISender sender,
        RegisterRequest request, HttpContext httpContext)
    {
        var result = await sender.Send(request.AsCommand());

        return result.Match(
            value => Results.Ok(value),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Registration failed.",
            },
            httpContext));
    }
}
