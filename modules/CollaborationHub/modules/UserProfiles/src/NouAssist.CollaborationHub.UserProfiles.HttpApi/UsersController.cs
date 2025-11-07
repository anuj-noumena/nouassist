using NouAssist.CollaborationHub.UserProfiles.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CollaborationHub.UserProfiles;

public abstract class UsersController : AbpControllerBase
{
    protected UsersController()
    {
        LocalizationResource = typeof(UserProfilesResource);
    }
}
