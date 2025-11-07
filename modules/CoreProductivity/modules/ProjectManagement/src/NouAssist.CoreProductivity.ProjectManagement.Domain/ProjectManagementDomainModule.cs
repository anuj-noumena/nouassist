using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.ProjectManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ProjectManagementDomainSharedModule)
)]
public class ProjectManagementDomainModule : AbpModule
{

}
