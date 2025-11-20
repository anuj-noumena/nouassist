using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore
{
    // Used by EF Core tools for migrations
    public class CollaborationDbContextFactory : IDesignTimeDbContextFactory<CollaborationDbContext>
    {
        public CollaborationDbContext CreateDbContext(string[] args)
        {
            // Get configuration from appsettings.json
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<CollaborationDbContext>()
                .UseNpgsql(configuration.GetConnectionString(CollaborationDbProperties.ConnectionStringName));

            return new CollaborationDbContext(builder.Options);
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
