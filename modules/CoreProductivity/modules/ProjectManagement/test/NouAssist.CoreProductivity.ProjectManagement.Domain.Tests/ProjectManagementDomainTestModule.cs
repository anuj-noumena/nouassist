using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.ProjectManagement;

[DependsOn(
    typeof(ProjectManagementDomainModule),
    typeof(ProjectManagementTestBaseModule)
)]
public class ProjectManagementDomainTestModule : AbpModule
{

}
