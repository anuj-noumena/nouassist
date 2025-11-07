using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.CoreProductivity.TimeManagement;

[DependsOn(
    typeof(TimeManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class TimeManagementApplicationContractsModule : AbpModule
{

}
