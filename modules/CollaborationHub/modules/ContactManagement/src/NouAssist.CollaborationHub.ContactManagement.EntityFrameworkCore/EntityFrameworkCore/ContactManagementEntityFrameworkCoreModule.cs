using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.ContactManagement.EntityFrameworkCore;

[DependsOn(
    typeof(ContactManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ContactManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ContactManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<IContactManagementDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
