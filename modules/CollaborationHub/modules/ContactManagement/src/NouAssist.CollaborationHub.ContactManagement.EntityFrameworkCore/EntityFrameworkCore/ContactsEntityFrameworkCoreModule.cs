using Microsoft.Extensions.DependencyInjection;
using NouAssist.CollaborationHub.ContactManagement.Aggregates;
using NouAssist.CollaborationHub.ContactManagement.Data;
using NouAssist.CollaborationHub.ContactManagement.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.ContactManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(ContactManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class ContactManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ContactsDbContext>(options =>
            {
                options.AddRepository<Contact, EfCoreContactRepository>();
                options.AddRepository<Client, EfCoreClientRepository>();
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
