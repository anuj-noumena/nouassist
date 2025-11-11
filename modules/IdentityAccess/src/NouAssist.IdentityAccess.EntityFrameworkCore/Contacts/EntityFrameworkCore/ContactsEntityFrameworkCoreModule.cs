using Microsoft.Extensions.DependencyInjection;
using NouAssist.IdentityAccess.Contacts.Aggregates;
using NouAssist.IdentityAccess.Contacts.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess.Contacts.EntityFrameworkCore
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
