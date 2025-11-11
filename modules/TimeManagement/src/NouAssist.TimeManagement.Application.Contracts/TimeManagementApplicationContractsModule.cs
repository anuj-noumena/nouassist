using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.TimeManagement;

[DependsOn(
    typeof(TimeManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class TimeManagementApplicationContractsModule : AbpModule
{

}
