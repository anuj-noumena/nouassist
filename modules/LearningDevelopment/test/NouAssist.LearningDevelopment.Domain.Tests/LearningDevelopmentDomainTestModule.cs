using Volo.Abp.Modularity;

namespace NouAssist.LearningDevelopment;

[DependsOn(
    typeof(LearningDevelopmentDomainModule),
    typeof(LearningDevelopmentTestBaseModule)
)]
public class LearningDevelopmentDomainTestModule : AbpModule
{

}
