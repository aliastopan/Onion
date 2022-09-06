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
        string jwt = request.Jwt;
        string refreshToken = request.RefreshToken;

        var refresh = _jwtService.Refresh(ref jwt, ref refreshToken);
        if(!refresh.IsSuccess)
        {
            return Result<RefreshResponse>.Inherit(result: refresh);
        }

        var response = new RefreshResponse(jwt, refreshToken);
        return Result<RefreshResponse>.Ok(response);
    }
}
