using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.SkillDevelopment
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(SkillDevelopmentDomainSharedModule)
    )]
    public class SkillDevelopmentDomainModule : AbpModule
    {
    }
}
