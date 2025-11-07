using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.GTD;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(GTDDomainSharedModule)
)]
public class GTDDomainModule : AbpModule
{

}
