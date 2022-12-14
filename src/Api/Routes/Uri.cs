namespace Onion.Api.Routes;

public static class Uri
{
    public static class Identity
    {
        public const string Tag = nameof(Identity);
        public const string Register = "/api/register";
        public const string Login = "/api/login";
        public const string Refresh = "/api/auth/refresh";
        public const string ResetPassword = "/api/auth/reset-password";
        public const string GrantRole = "/api/auth/grant-role";
    }
}
