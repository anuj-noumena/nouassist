using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.TaskManagement;

[DependsOn(
    typeof(TaskManagementDomainModule),
    typeof(TaskManagementTestBaseModule)
)]
public class TaskManagementDomainTestModule : AbpModule
{

}
