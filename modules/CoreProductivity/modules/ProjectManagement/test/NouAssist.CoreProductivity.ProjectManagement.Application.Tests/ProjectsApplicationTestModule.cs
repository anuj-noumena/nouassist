using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.ProjectManagement;

[DependsOn(
    typeof(ProjectManagementApplicationModule),
    typeof(ProjectManagementDomainTestModule)
    )]
public class ProjectManagementApplicationTestModule : AbpModule
{

}
