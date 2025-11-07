using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CoreProductivityDomainSharedModule)
)]
public class CoreProductivityDomainModule : AbpModule
{

}
