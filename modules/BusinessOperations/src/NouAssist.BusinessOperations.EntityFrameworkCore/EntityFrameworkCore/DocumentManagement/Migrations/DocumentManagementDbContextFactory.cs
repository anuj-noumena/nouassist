using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NouAssist.BusinessOperations.DocumentManagement.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.DocumentManagement.Migrations
{
    public class DocumentManagementDbContextFactory : IDesignTimeDbContextFactory<DocumentManagementDbContext>
    {
        public DocumentManagementDbContext CreateDbContext(string[] args)
        {
            // Get configuration from appsettings.json
            var configuration = BuildConfiguration();
            
            var builder = new DbContextOptionsBuilder<DocumentManagementDbContext>()
                .UseNpgsql(configuration.GetConnectionString(DocumentManagementDbProperties.ConnectionStringName));
            
            return new DocumentManagementDbContext(builder.Options);
        }
        
        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Host/NouAssist.Host"))
                .AddJsonFile("appsettings.json", optional: false);
            
            return builder.Build();
        }
    }
}
