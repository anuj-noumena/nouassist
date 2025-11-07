using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.ReflectionAndSupport.Journaling;

[DependsOn(
    typeof(JournalingDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class JournalingApplicationContractsModule : AbpModule
{

}
