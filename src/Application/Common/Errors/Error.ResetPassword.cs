namespace Onion.Application.Common.Errors;

public partial class Error
{
    public static class ResetPassword
    {
        public readonly static ErrorResult IncorrectPassword = new("Incorrect password.");
        public readonly static ErrorResult SameOldPassword = new("New password cannot be same as the old one.");
    }
}
