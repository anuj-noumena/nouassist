using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement;

[DependsOn(
    typeof(WorkManagementDomainModule),
    typeof(WorkManagementTestBaseModule)
)]
public class WorkManagementDomainTestModule : AbpModule
{

}
