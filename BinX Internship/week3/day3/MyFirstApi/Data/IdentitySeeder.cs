using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MyFirstApi.Authorization;

namespace MyFirstApi.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

        await EnsureRoleAsync(
            roleManager,
            AppRoles.User);

        await EnsureRoleAsync(
            roleManager,
            AppRoles.Admin);

        var userEmail = GetRequiredSetting(
            configuration,
            "SeedUsers:User:Email");

        var userPassword = GetRequiredSetting(
            configuration,
            "SeedUsers:User:Password");

        var adminEmail = GetRequiredSetting(
            configuration,
            "SeedUsers:Admin:Email");

        var adminPassword = GetRequiredSetting(
            configuration,
            "SeedUsers:Admin:Password");

        var regularUser = await EnsureUserAsync(
            userManager,
            userEmail,
            userPassword);

        await EnsureUserRoleAsync(
            userManager,
            regularUser,
            AppRoles.User);

        var adminUser = await EnsureUserAsync(
            userManager,
            adminEmail,
            adminPassword);

        await EnsureUserRoleAsync(
            userManager,
            adminUser,
            AppRoles.Admin);

        await EnsureUserClaimAsync(
            userManager,
            adminUser,
            new Claim(
                AppClaimTypes.Permission,
                AppPermissions.BooksCreate));
    }

    private static async Task EnsureRoleAsync(
        RoleManager<IdentityRole> roleManager,
        string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
        {
            return;
        }

        var result = await roleManager.CreateAsync(
            new IdentityRole(roleName));

        EnsureSucceeded(
            result,
            $"Creating role '{roleName}'");
    }

    private static async Task<IdentityUser> EnsureUserAsync(
        UserManager<IdentityUser> userManager,
        string email,
        string password)
    {
        var existingUser =
            await userManager.FindByEmailAsync(email);

        if (existingUser is not null)
        {
            return existingUser;
        }

        var user = new IdentityUser
        {
            UserName = email,
            Email = email
        };

        var result = await userManager.CreateAsync(
            user,
            password);

        EnsureSucceeded(
            result,
            $"Creating user '{email}'");

        return user;
    }

    private static async Task EnsureUserRoleAsync(
        UserManager<IdentityUser> userManager,
        IdentityUser user,
        string roleName)
    {
        if (await userManager.IsInRoleAsync(
                user,
                roleName))
        {
            return;
        }

        var result = await userManager.AddToRoleAsync(
            user,
            roleName);

        EnsureSucceeded(
            result,
            $"Assigning role '{roleName}' to '{user.Email}'");
    }

    private static async Task EnsureUserClaimAsync(
        UserManager<IdentityUser> userManager,
        IdentityUser user,
        Claim claim)
    {
        var existingClaims =
            await userManager.GetClaimsAsync(user);

        var claimExists = existingClaims.Any(
            existingClaim =>
                existingClaim.Type == claim.Type &&
                existingClaim.Value == claim.Value);

        if (claimExists)
        {
            return;
        }

        var result = await userManager.AddClaimAsync(
            user,
            claim);

        EnsureSucceeded(
            result,
            $"Adding permission to '{user.Email}'");
    }

    private static string GetRequiredSetting(
        IConfiguration configuration,
        string key)
    {
        return configuration[key]
            ?? throw new InvalidOperationException(
                $"Required setting '{key}' is missing.");
    }

    private static void EnsureSucceeded(
        IdentityResult result,
        string operation)
    {
        if (result.Succeeded)
        {
            return;
        }

        var errors = string.Join(
            "; ",
            result.Errors.Select(error =>
                $"{error.Code}: {error.Description}"));

        throw new InvalidOperationException(
            $"{operation} failed: {errors}");
    }
}