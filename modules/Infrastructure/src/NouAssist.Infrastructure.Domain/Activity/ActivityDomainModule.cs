using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.Activity;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ActivityDomainSharedModule)
)]
public class ActivityDomainModule : AbpModule
{

}
