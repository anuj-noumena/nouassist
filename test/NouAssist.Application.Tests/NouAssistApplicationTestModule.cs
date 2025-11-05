using Volo.Abp.Modularity;

namespace NouAssist;

[DependsOn(
    typeof(NouAssistApplicationModule),
    typeof(NouAssistDomainTestModule)
)]
public class NouAssistApplicationTestModule : AbpModule
{

}
