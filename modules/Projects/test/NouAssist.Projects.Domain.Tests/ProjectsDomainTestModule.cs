using Volo.Abp.Modularity;

namespace NouAssist.Projects;

[DependsOn(
    typeof(ProjectsDomainModule),
    typeof(ProjectsTestBaseModule)
)]
public class ProjectsDomainTestModule : AbpModule
{

}
