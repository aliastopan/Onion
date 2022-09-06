using Onion.Contracts.Identity;

namespace Onion.Api.Routes.Identity;

public class AuthenticationEndpoint : IRouteEndpoint
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(Uri.Identity.Login, Login)
           .AllowAnonymous();
    }

    internal async Task<IResult> Login([FromServices] ISender sender,
        LoginRequest request, HttpContext httpContext)
    {
        var result = await sender.Send(request.AsCommand());

        return result.Match(
            value =>
            {
                var cookieOption = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMonths(1)
                };
                httpContext.Response.Cookies.Append("jwt", value.Jwt, cookieOption);
                return Results.Ok(value);
            },
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Login failed.",
            },
            httpContext));
    }
}
