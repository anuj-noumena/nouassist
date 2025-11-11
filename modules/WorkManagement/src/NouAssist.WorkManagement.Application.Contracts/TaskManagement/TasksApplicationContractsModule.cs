using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.TaskManagement;

[DependsOn(
    typeof(TaskManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class TaskManagementApplicationContractsModule : AbpModule
{

}
