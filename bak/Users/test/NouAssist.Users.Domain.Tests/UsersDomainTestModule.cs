using Volo.Abp.Modularity;

namespace NouAssist.Users;

[DependsOn(
    typeof(UsersDomainModule),
    typeof(UsersTestBaseModule)
)]
public class UsersDomainTestModule : AbpModule
{

}
