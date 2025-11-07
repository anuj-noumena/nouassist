using NouAssist.Users.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Users;

public abstract class UsersController : AbpControllerBase
{
    protected UsersController()
    {
        LocalizationResource = typeof(UsersResource);
    }
}
