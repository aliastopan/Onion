namespace Onion.Application.Identity.Commands.Registration;

public record RegisterCommand : IRequest<Result<RegisterCommandResponse>>
{
    [Required]
    public string Username { get; init; }

    [Required]
    public string Email { get; init; }

    [Required]
    public string Password { get; init; }

    public RegisterCommand(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }
}
