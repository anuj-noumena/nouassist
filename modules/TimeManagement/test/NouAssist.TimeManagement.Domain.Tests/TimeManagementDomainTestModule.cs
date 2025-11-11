using Volo.Abp.Modularity;

namespace NouAssist.TimeManagement;

[DependsOn(
    typeof(TimeManagementDomainModule),
    typeof(TimeManagementTestBaseModule)
)]
public class TimeManagementDomainTestModule : AbpModule
{

}
