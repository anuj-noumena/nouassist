using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess;

[DependsOn(
    typeof(IdentityAccessDomainModule),
    typeof(IdentityAccessTestBaseModule)
)]
public class IdentityAccessDomainTestModule : AbpModule
{

}
