using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.ContactManagement;

[DependsOn(
    typeof(ContactManagementDomainModule),
    typeof(ContactManagementTestBaseModule)
)]
public class ContactManagementDomainTestModule : AbpModule
{

}
