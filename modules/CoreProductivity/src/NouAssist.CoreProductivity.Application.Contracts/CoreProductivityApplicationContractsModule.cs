using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.CoreProductivity;

[DependsOn(
    typeof(CoreProductivityDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class CoreProductivityApplicationContractsModule : AbpModule
{

}
