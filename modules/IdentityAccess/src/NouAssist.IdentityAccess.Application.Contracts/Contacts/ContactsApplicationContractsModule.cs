using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess.Contacts;

[DependsOn(
    typeof(ContactsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ContactsApplicationContractsModule : AbpModule
{

}
