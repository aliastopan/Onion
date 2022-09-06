namespace Onion.Application.Identity.Commands.Authentication;

public record LoginCommand : IRequest<Result<LoginResponse>>
{
    public LoginCommand(string username, string password)
    {
        Username = username;
        Password = password;
    }

    [Required] public string Username { get; init; }
    [Required] public string Password { get; init; }
}
