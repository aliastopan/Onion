using Onion.Application.Common.Validations;

namespace Onion.Application.Identity.Commands.Registration;

public record RegisterCommand : IRequest<Result<RegisterResponse>>
{
    public RegisterCommand(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }

    [Required]
    [RegularExpression(RegexPattern.Username)]
    public string Username { get; init; }

    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [Required]
    [RegularExpression(RegexPattern.StrongPassword)]
    public string Password { get; init; }
}
