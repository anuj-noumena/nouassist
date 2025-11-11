using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess.Users;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(UsersDomainSharedModule)
)]
public class UsersDomainModule : AbpModule
{

}
