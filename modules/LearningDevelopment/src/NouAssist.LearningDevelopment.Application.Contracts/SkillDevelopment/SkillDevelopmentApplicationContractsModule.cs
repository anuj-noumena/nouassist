using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.SkillDevelopment
{
    [DependsOn(
        typeof(SkillDevelopmentDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
    )]
    public class SkillDevelopmentApplicationContractsModule : AbpModule
    {
    }
}
