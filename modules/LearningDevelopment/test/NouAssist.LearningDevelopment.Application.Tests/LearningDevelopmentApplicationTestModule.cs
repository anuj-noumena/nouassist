using Volo.Abp.Modularity;

namespace NouAssist.LearningDevelopment;

[DependsOn(
    typeof(LearningDevelopmentApplicationModule),
    typeof(LearningDevelopmentDomainTestModule)
    )]
public class LearningDevelopmentApplicationTestModule : AbpModule
{

}
