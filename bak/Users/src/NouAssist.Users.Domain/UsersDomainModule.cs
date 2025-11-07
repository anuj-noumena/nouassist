using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Users;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(UsersDomainSharedModule)
)]
public class UsersDomainModule : AbpModule
{

}
