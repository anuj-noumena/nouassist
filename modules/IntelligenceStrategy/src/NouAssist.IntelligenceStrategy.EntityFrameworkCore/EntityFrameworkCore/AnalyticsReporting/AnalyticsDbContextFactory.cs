using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace NouAssist.Analytics.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class AnalyticsDbContextFactory : IDesignTimeDbContextFactory<AnalyticsDbContext>
    {
        public AnalyticsDbContext CreateDbContext(string[] args)
        {
            // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-behavior
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<AnalyticsDbContext>()
                .UseNpgsql(configuration.GetConnectionString("Analytics"));

            return new AnalyticsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../NouAssist.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
