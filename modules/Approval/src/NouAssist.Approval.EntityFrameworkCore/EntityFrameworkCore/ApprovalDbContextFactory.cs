using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace NouAssist.Approval.EntityFrameworkCore
{
    public class ApprovalDbContextFactory : IDesignTimeDbContextFactory<ApprovalDbContext>
    {
        public ApprovalDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ApprovalDbContext>()
                .UseNpgsql(configuration.GetConnectionString("Approval"));

            return new ApprovalDbContext(builder.Options);
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
