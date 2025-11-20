using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship;

[DependsOn(
    typeof(StartupEntrepreneurshipApplicationModule),
    typeof(StartupEntrepreneurshipDomainTestModule)
    )]
public class StartupEntrepreneurshipApplicationTestModule : AbpModule
{

}
