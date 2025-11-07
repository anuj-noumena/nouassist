using NouAssist.CollaborationHub.UserProfiles.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.UserProfiles;

public abstract class UsersAppService : ApplicationService
{
    protected UsersAppService()
    {
        LocalizationResource = typeof(UserProfilesResource);
        ObjectMapperContext = typeof(UserProfilesApplicationModule);
    }
}
