using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.UserProfiles;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(UserProfilesDomainSharedModule)
)]
public class UserProfilesDomainModule : AbpModule
{

}
