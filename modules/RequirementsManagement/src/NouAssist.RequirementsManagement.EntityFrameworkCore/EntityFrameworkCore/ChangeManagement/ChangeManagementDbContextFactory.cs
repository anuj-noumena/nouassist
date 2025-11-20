using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace NouAssist.RequirementsManagement.ChangeManagement.EntityFrameworkCore
{
    /// <summary>
    /// Factory for creating DbContext instances during design-time (for migrations)
    /// </summary>
    public class ChangeManagementDbContextFactory : IDesignTimeDbContextFactory<ChangeManagementDbContext>
    {
        public ChangeManagementDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ChangeManagementDbContext>()
                .UseNpgsql(
                    configuration.GetConnectionString(ChangeManagementDbProperties.ConnectionStringName),
                    optionsBuilder => optionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", ChangeManagementDbProperties.DbSchema)
                );

            return new ChangeManagementDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
