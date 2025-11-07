using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.ContactManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ContactManagementDomainSharedModule)
)]
public class ContactManagementDomainModule : AbpModule
{

}
