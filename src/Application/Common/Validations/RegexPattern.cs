namespace Onion.Application.Common.Validations;

public static class RegexPattern
{
    public const string Username = "^(?=.{3,16}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$";
    public const string StrongPassword = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$";
}