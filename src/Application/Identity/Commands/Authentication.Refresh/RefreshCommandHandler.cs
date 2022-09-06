namespace Onion.Application.Identity.Commands.Authentication.Refresh;

public class RefreshCommandHandler
    : IRequestHandler<RefreshCommand, Result<RefreshResponse>>
{
    private readonly IJwtService _jwtService;

    public RefreshCommandHandler(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public Task<Result<RefreshResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var result = Refresh(request);
        return Task.FromResult(result);
    }

    internal Result<RefreshResponse> Refresh(RefreshCommand request)
    {
        var refresh = _jwtService.Refresh(request.Jwt, request.RefreshToken);
        if(!refresh.IsSuccess)
        {
            return Result<RefreshResponse>.Inherit(result: refresh);
        }

        (string jwt, string refreshToken) value = refresh;
        var response = new RefreshResponse(value.jwt, value.refreshToken);
        return Result<RefreshResponse>.Ok(response);
    }
}
