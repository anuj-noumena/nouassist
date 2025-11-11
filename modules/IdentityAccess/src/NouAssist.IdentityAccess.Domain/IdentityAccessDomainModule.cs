using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(IdentityAccessDomainSharedModule)
)]
public class IdentityAccessDomainModule : AbpModule
{

}
