using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration,
        string contentRootPath)
    {
        string connectionString = BuildSqliteConnectionString(
            configuration.GetConnectionString("DefaultConnection"),
            contentRootPath);

        services.AddDbContext<DataContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<DataContext>());

        return services;
    }

    private static string BuildSqliteConnectionString(string? connectionString, string contentRootPath)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        SqliteConnectionStringBuilder builder = new(connectionString);

        if (builder.DataSource is ":memory:" || string.IsNullOrWhiteSpace(builder.DataSource))
        {
            return builder.ToString();
        }

        if (!Path.IsPathFullyQualified(builder.DataSource))
        {
            builder.DataSource = Path.GetFullPath(
                Path.Combine(contentRootPath, builder.DataSource));
        }

        string? directory = Path.GetDirectoryName(builder.DataSource);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return builder.ToString();
    }
}
