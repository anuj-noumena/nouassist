using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.UserProfiles;

[DependsOn(
    typeof(UserProfilesApplicationModule),
    typeof(UserProfilesDomainTestModule)
    )]
public class UserProfilesApplicationTestModule : AbpModule
{

}
