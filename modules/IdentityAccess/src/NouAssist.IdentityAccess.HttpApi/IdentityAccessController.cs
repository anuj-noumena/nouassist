using NouAssist.IdentityAccess.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.IdentityAccess;

public abstract class IdentityAccessController : AbpControllerBase
{
    protected IdentityAccessController()
    {
        LocalizationResource = typeof(IdentityAccessResource);
    }
}
