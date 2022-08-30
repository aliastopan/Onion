namespace Onion.Application.Identity.Commands.Registration;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Result<RegisterCommandResponse>>
{
    public Task<Result<RegisterCommandResponse>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        Result<RegisterCommandResponse> result;
        var isValid = request.TryValidate(out var errors);
        if(!isValid)
        {
            result = Result<RegisterCommandResponse>.Invalid(errors);
            return Task.FromResult(result);
        }

        var response = new RegisterCommandResponse(Guid.NewGuid(), request.Username);
        result = Result<RegisterCommandResponse>.Ok(response);
        return Task.FromResult(result);
    }
}
