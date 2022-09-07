using Onion.Application.Common.Validations;

namespace Onion.Application.Identity.Commands.ResetPassword;

public record ResetPasswordCommand : IRequest<Result<ResetPasswordResponse>>
{
    public ResetPasswordCommand(Guid userId, string oldPassword, string newPassword, string repeatPassword)
    {
        UserId = userId;
        OldPassword = oldPassword;
        NewPassword = newPassword;
        RepeatPassword = repeatPassword;
    }

    [Required]
    public Guid UserId { get; init; }

    [Required]
    public string OldPassword { get; init; }

    [Required]
    [Compare(nameof(RepeatPassword))]
    [RegularExpression(RegexPattern.StrongPassword)]
    public string NewPassword { get; init; }

    [Required]
    public string RepeatPassword { get; init; }
}
