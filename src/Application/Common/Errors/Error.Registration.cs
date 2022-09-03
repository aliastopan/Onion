namespace Onion.Application.Common.Errors;

public partial class Error
{
    public static class Registration
    {
        public readonly static ErrorResult UsernameTaken = new("Username is already taken.");
        public readonly static ErrorResult EmailInUse = new("Email is already in use.");
    }
}
