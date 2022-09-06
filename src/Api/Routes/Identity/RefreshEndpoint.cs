using System.Net;
using Onion.Application.Identity.Commands.Authentication.Refresh;

namespace Onion.Api.Routes.Identity;

public class RefreshEndpoint : IRouteEndpoint
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(Uri.Identity.Refresh, Refresh)
           .AllowAnonymous()
           .WithTags(Uri.Identity.Tag);
    }

    internal async Task<IResult> Refresh([FromServices] ISender sender,
        HttpContext httpContext)
    {
        var jwt = httpContext.Request.Cookies["jwt"];
        var rwt = httpContext.Request.Cookies["rwt"];
        if(jwt is null || rwt is null)
        {
            return Results.Problem(new ProblemDetails
            {
                Status = (int)HttpStatusCode.NotFound
            });
        }

        var command = new RefreshCommand(jwt, rwt);
        var result = await sender.Send(command);

        return result.Match(
            value =>
            {
                var cookieOption = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMonths(1)
                };
                httpContext.Response.Cookies.Append("jwt", value.Jwt, cookieOption);
                httpContext.Response.Cookies.Append("rwt", value.RefreshToken, cookieOption);
                return Results.Ok(value);
            },
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Refresh failed.",
            },
            httpContext));
    }
}
