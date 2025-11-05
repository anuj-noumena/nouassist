using Volo.Abp.Modularity;

namespace NouAssist.Users;

[DependsOn(
    typeof(UsersApplicationModule),
    typeof(UsersDomainTestModule)
    )]
public class UsersApplicationTestModule : AbpModule
{

}
