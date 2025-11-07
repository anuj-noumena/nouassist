using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.CollaborationHub.ContactManagement;

[DependsOn(
    typeof(ContactManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ContactManagementApplicationContractsModule : AbpModule
{

}
