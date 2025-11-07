using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.TaskManagement;

[DependsOn(
    typeof(TaskManagementApplicationModule),
    typeof(TaskManagementDomainTestModule)
    )]
public class TaskManagementApplicationTestModule : AbpModule
{

}
