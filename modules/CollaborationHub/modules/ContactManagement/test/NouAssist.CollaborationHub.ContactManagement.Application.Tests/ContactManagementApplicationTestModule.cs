using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.ContactManagement;

[DependsOn(
    typeof(ContactManagementApplicationModule),
    typeof(ContactManagementDomainTestModule)
    )]
public class ContactManagementApplicationTestModule : AbpModule
{

}
