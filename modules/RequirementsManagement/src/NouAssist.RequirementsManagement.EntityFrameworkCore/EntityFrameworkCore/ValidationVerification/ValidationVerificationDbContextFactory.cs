using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore
{
    /// <summary>
    /// Factory for ValidationVerificationDbContext to support EF Core migrations design-time creation
    /// This is only used for migrations and design-time operations
    /// </summary>
    public class ValidationVerificationDbContextFactory : IDesignTimeDbContextFactory<ValidationVerificationDbContext>
    {
        public ValidationVerificationDbContext CreateDbContext(string[] args)
        {
            // Build configuration
            var configuration = BuildConfiguration();

            // Configure DbContext options
            var builder = new DbContextOptionsBuilder<ValidationVerificationDbContext>()
                .UseNpgsql(configuration.GetConnectionString(ValidationVerificationDbProperties.ConnectionStringName));

            return new ValidationVerificationDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
