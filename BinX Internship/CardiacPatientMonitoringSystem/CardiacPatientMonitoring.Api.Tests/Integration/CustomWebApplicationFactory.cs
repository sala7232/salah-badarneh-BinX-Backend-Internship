using CardiacPatientMonitoring.Api.Data;
using CardiacPatientMonitoring.Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CardiacPatientMonitoring.Api.Tests.Integration;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    private readonly string _databaseName =
        $"CardiacIntegrationTests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            var testSettings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:CardiacDatabase"] =
                    "Server=(local);Database=IntegrationTests;",
                ["Jwt:Issuer"] = TestJwtTokenFactory.Issuer,
                ["Jwt:Audience"] = TestJwtTokenFactory.Audience,
                ["Jwt:Key"] = TestJwtTokenFactory.SigningKey,
                ["Jwt:ExpiryMinutes"] = "15"
            };

            configuration.AddInMemoryCollection(testSettings);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<
                DbContextOptions<CardiacDbContext>>();
            services.RemoveAll<
                IDbContextOptionsConfiguration<
                    CardiacDbContext>>();
            services.RemoveAll<CardiacDbContext>();

            services.AddDbContext<CardiacDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }

    public async Task ResetDatabaseAsync(
        params Patient[] patients)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider
            .GetRequiredService<CardiacDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        if (patients.Length == 0)
        {
            return;
        }

        await context.Patients.AddRangeAsync(patients);
        await context.SaveChangesAsync();
    }
}
