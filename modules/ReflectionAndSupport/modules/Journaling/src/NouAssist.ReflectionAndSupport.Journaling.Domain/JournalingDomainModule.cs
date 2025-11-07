using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Journaling;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(JournalingDomainSharedModule)
)]
public class JournalingDomainModule : AbpModule
{

}
