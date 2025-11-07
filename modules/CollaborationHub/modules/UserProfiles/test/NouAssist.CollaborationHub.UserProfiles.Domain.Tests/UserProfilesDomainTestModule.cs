using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.UserProfiles;

[DependsOn(
    typeof(UserProfilesDomainModule),
    typeof(UserProfilesTestBaseModule)
)]
public class UserProfilesDomainTestModule : AbpModule
{

}
