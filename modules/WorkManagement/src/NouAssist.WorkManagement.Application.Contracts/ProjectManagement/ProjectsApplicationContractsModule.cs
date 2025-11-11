using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.ProjectManagement;

[DependsOn(
    typeof(ProjectManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ProjectManagementApplicationContractsModule : AbpModule
{

}
