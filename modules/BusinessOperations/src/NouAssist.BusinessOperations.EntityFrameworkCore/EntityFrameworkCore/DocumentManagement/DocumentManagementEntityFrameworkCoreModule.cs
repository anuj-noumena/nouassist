using Microsoft.Extensions.DependencyInjection;
using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.EntityFrameworkCore.Repositories;
using NouAssist.BusinessOperations.DocumentManagement.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations.DocumentManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(DocumentManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class DocumentManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<DocumentManagementDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<Document, EfCoreDocumentRepository>();
                
                // Add default repositories for all entities
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            // Configure EF Core to use PostgreSQL
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
