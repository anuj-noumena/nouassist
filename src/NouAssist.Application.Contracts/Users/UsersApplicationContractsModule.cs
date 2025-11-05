using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Users
{
    [DependsOn(
        typeof(UsersDomainModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
    )]
    public class UsersApplicationContractsModule : AbpModule
    {
    }
}
