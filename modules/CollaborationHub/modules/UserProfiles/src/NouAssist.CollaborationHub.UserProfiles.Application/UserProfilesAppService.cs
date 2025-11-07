using NouAssist.CollaborationHub.UserProfiles.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.UserProfiles;

public abstract class UserProfilesAppService : ApplicationService
{
    protected UserProfilesAppService()
    {
        LocalizationResource = typeof(UserProfilesResource);
        ObjectMapperContext = typeof(UserProfilesApplicationModule);
    }
}
