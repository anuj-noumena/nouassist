using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.TaskManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(TaskManagementDomainSharedModule)
)]
public class TaskManagementDomainModule : AbpModule
{

}
