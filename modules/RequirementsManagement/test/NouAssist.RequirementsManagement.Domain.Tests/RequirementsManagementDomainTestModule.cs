using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement;

[DependsOn(
    typeof(RequirementsManagementDomainModule),
    typeof(RequirementsManagementTestBaseModule)
)]
public class RequirementsManagementDomainTestModule : AbpModule
{

}
