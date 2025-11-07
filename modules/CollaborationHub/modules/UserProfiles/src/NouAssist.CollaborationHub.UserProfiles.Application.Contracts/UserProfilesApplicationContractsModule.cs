using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.CollaborationHub.UserProfiles;

[DependsOn(
    typeof(UserProfilesDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class UserProfilesApplicationContractsModule : AbpModule
{

}
