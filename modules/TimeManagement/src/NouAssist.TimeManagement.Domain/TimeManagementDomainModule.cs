using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.TimeManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(TimeManagementDomainSharedModule)
)]
public class TimeManagementDomainModule : AbpModule
{

}
