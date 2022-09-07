namespace Onion.Contracts.Identity;

public record ResetPasswordRequest(
    Guid UserId,
    string OldPassword,
    string NewPassword,
    string RepeatPassword);
