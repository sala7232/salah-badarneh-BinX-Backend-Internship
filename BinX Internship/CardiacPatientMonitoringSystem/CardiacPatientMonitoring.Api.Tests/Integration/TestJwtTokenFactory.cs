using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CardiacPatientMonitoring.Api.Tests.Integration;

internal static class TestJwtTokenFactory
{
    internal const string Issuer =
        "CardiacPatientMonitoringApi.IntegrationTests";

    internal const string Audience =
        "CardiacPatientMonitoringApi.IntegrationTests.Client";

    internal const string SigningKey =
        "Cardiac-Integration-Tests-Jwt-Key-2026-Only";

    internal static string CreateToken()
    {
        var now = DateTime.UtcNow;

        var claims = new[]
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                "integration-test-user"),
            new Claim(
                JwtRegisteredClaimNames.Email,
                "integration@test.local"),
            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(SigningKey));

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            notBefore: now.AddMinutes(-1),
            expires: now.AddMinutes(15),
            signingCredentials: new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
