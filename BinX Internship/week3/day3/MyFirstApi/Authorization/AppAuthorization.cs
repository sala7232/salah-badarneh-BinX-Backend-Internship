namespace MyFirstApi.Authorization;

public static class AppRoles
{
    public const string User = "User";
    public const string Admin = "Admin";
}

public static class AppPolicies
{
    public const string CanCreateBooks = "CanCreateBooks";
}

public static class AppClaimTypes
{
    public const string Role = "role";
    public const string Permission = "permission";
}

public static class AppPermissions
{
    public const string BooksCreate = "books.create";
}

public static class AppRateLimitPolicies
{
    public const string Login = "LoginRateLimit";
}

public static class AppCorsPolicies
{
    public const string AllowFrontend = "AllowFrontend";
}