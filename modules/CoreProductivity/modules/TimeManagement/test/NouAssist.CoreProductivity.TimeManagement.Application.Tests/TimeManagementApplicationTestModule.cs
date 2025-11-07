using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.TimeManagement;

[DependsOn(
    typeof(TimeManagementApplicationModule),
    typeof(TimeManagementDomainTestModule)
    )]
public class TimeManagementApplicationTestModule : AbpModule
{

}
