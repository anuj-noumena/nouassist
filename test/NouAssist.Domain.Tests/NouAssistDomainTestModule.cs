using Volo.Abp.Modularity;

namespace NouAssist;

[DependsOn(
    typeof(NouAssistDomainModule),
    typeof(NouAssistTestBaseModule)
)]
public class NouAssistDomainTestModule : AbpModule
{

}
