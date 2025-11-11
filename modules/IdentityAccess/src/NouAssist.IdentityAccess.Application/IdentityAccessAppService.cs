using NouAssist.IdentityAccess.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.IdentityAccess;

public abstract class IdentityAccessAppService : ApplicationService
{
    protected IdentityAccessAppService()
    {
        LocalizationResource = typeof(IdentityAccessResource);
        ObjectMapperContext = typeof(IdentityAccessApplicationModule);
    }
}
