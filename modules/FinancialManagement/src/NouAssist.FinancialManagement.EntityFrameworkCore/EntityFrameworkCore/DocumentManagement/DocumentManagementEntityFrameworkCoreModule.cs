using Microsoft.Extensions.DependencyInjection;
using NouAssist.DocumentManagement.EntityFrameworkCore.Repositories;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.DocumentManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(DocumentManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class DocumentManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<DocumentManagementDbContext>(options =>
            {
                options.AddRepository<Document, EfCoreDocumentRepository>();
                options.AddRepository<DocumentFolder, EfCoreDocumentFolderRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
