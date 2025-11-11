using NouAssist.IdentityAccess.Users.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.IdentityAccess.Users;

public abstract class UsersAppService : ApplicationService
{
    protected UsersAppService()
    {
        LocalizationResource = typeof(UsersResource);
        ObjectMapperContext = typeof(UsersApplicationModule);
    }
}
