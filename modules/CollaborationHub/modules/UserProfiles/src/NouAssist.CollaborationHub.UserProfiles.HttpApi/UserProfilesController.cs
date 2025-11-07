using NouAssist.CollaborationHub.UserProfiles.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CollaborationHub.UserProfiles;

public abstract class UserProfilesController : AbpControllerBase
{
    protected UserProfilesController()
    {
        LocalizationResource = typeof(UserProfilesResource);
    }
}
