using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Journaling;

[DependsOn(
    typeof(JournalingDomainModule),
    typeof(JournalingTestBaseModule)
)]
public class JournalingDomainTestModule : AbpModule
{

}
