using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.LearningDevelopment;

[DependsOn(
    typeof(LearningDevelopmentDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class LearningDevelopmentApplicationContractsModule : AbpModule
{

}
