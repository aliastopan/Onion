namespace Onion.Application.Common.Errors;

public partial class Error
{
    public static class GrantRole
    {
        public readonly static ErrorResult Denied = new("You don't have permission.");
    }
}
