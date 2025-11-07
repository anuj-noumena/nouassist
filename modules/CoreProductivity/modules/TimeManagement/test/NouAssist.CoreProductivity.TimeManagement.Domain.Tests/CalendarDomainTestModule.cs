using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.TimeManagement;

[DependsOn(
    typeof(TimeManagementDomainModule),
    typeof(TimeManagementTestBaseModule)
)]
public class TimeManagementDomainTestModule : AbpModule
{

}
