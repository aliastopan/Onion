namespace Onion.Application.Common.Errors;

public partial class Error
{
    public static class Authentication
    {
        public readonly static ErrorResult UserNotRegistered = new("Username is not registered.");
        public readonly static ErrorResult IncorrectPassword = new("Incorrect password.");
    }
}
