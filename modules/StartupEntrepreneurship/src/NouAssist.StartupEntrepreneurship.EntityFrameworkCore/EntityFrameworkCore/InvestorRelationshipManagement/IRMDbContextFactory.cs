using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore
{
    /// <summary>
    /// Design-time factory for EF Core migrations
    /// </summary>
    public class IRMDbContextFactory : IDesignTimeDbContextFactory<IRMDbContext>
    {
        public IRMDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var connectionString = configuration.GetConnectionString(IRMDbProperties.ConnectionStringName) 
                                 ?? configuration.GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<IRMDbContext>()
                .UseNpgsql(connectionString, options =>
                {
                    options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });

            return new IRMDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
