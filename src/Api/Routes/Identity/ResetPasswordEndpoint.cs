using Onion.Contracts.Identity;

namespace Onion.Api.Routes.Identity;

public class ResetPasswordEndpoint : IRouteEndpoint
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(Uri.Identity.ResetPassword, ResetPassword)
           .WithTags(Uri.Identity.Tag);
    }

    internal async Task<IResult> ResetPassword([FromServices] ISender sender,
        ResetPasswordRequest request, HttpContext httpContext)
    {
        var result = await sender.Send(request.AsCommand());

        return result.Match(
            value => Results.Ok(value),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to reset password."
            },
            httpContext));
    }
}
