using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(StartupEntrepreneurshipDomainSharedModule)
)]
public class StartupEntrepreneurshipDomainModule : AbpModule
{

}
