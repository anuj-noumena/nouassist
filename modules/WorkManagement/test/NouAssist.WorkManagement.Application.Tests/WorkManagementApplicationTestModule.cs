using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement;

[DependsOn(
    typeof(WorkManagementApplicationModule),
    typeof(WorkManagementDomainTestModule)
    )]
public class WorkManagementApplicationTestModule : AbpModule
{

}
