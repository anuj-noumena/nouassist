using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Journaling;

[DependsOn(
    typeof(JournalingApplicationModule),
    typeof(JournalingDomainTestModule)
    )]
public class JournalingApplicationTestModule : AbpModule
{

}
