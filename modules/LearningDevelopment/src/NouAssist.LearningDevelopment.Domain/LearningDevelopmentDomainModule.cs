using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.LearningDevelopment;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(LearningDevelopmentDomainSharedModule)
)]
public class LearningDevelopmentDomainModule : AbpModule
{

}
