using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess.Users;

[DependsOn(
    typeof(UsersDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class UsersApplicationContractsModule : AbpModule
{

}
