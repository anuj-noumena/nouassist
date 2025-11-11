using Volo.Abp.Modularity;

namespace NouAssist.TimeManagement;

[DependsOn(
    typeof(TimeManagementApplicationModule),
    typeof(TimeManagementDomainTestModule)
    )]
public class TimeManagementApplicationTestModule : AbpModule
{

}
