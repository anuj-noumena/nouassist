using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * like Add-Migration and Update-Database */
    public class QAMigrationsDbContextFactory : IDesignTimeDbContextFactory<QAMigrationsDbContext>
    {
        public QAMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<QAMigrationsDbContext>()
                .UseNpgsql(configuration.GetConnectionString("QA"));

            return new QAMigrationsDbContext(builder.Options);
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
