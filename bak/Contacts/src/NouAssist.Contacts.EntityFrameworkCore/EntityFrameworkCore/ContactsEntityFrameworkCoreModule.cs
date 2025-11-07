using Microsoft.Extensions.DependencyInjection;
using NouAssist.Contacts.Aggregates;
using NouAssist.Contacts.Data;
using NouAssist.Contacts.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.Contacts.EntityFrameworkCore
{
    [DependsOn(
        typeof(ContactsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class ContactsEntityFrameworkCoreModule : AbpModule
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
