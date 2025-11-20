using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore
{
    /// <summary>
    /// Design-time factory for creating StrategicPlanningDbContext instances
    /// Used for migrations and other design-time operations
    /// </summary>
    public class StrategicPlanningDbContextFactory : IDesignTimeDbContextFactory<StrategicPlanningDbContext>
    {
        public StrategicPlanningDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var connectionString = configuration.GetConnectionString(StrategicPlanningDbProperties.ConnectionStringName) 
                                  ?? configuration.GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<StrategicPlanningDbContext>()
                .UseNpgsql(connectionString, options =>
                {
                    options.MigrationsHistoryTable("__StrategicPlanningMigrations");
                });

            return new StrategicPlanningDbContext(builder.Options);
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
