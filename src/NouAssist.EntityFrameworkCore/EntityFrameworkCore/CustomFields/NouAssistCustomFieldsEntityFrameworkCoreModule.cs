using Microsoft.Extensions.DependencyInjection;
using NouAssist.CustomFields.Aggregates;
using NouAssist.CustomFields.Data;
using NouAssist.CustomFields.Entities;
using NouAssist.CustomFields.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.CustomFields.EntityFrameworkCore
{
    [DependsOn(
        typeof(NouAssistCustomFieldsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class NouAssistCustomFieldsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CustomFieldsDbContext>(options =>
            {
                options.AddRepository<CustomFieldDefinition, EfCoreCustomFieldDefinitionRepository>();
                options.AddRepository<CustomFieldValue, EfCoreCustomFieldValueRepository>();

                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
