using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore
{
    /* This class is used for development tools (e.g. EF Core migrations) to create a DbContext with the configuration
     * needed for the FreelanceOperations module */
    public class FreelanceOperationsDbContextFactory : IDesignTimeDbContextFactory<FreelanceOperationsDbContext>
    {
        public FreelanceOperationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<FreelanceOperationsDbContext>()
                .UseNpgsql(configuration.GetConnectionString(FreelanceOperationsDbProperties.ConnectionStringName));

            return new FreelanceOperationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../NouAssist.Infrastructure.DbMigrations/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
