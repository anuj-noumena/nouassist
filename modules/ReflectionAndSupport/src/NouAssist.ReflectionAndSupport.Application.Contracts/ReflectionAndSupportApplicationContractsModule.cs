using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.ReflectionAndSupport;

[DependsOn(
    typeof(ReflectionAndSupportDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ReflectionAndSupportApplicationContractsModule : AbpModule
{

}
