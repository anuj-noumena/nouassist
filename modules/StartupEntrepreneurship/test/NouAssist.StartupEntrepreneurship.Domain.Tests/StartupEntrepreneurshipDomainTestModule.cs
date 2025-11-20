using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship;

[DependsOn(
    typeof(StartupEntrepreneurshipDomainModule),
    typeof(StartupEntrepreneurshipTestBaseModule)
)]
public class StartupEntrepreneurshipDomainTestModule : AbpModule
{

}
