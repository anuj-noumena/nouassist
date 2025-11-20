using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement;

[DependsOn(
    typeof(RequirementsManagementApplicationModule),
    typeof(RequirementsManagementDomainTestModule)
    )]
public class RequirementsManagementApplicationTestModule : AbpModule
{

}
