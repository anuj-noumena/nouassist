using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.Activity;

[DependsOn(
    typeof(ActivityDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ActivityApplicationContractsModule : AbpModule
{

}
