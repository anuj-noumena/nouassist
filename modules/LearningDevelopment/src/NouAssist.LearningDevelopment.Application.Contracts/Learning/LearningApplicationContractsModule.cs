using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Learning
{
    [DependsOn(
        typeof(LearningDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
    )]
    public class LearningApplicationContractsModule : AbpModule
    {
    }
}
