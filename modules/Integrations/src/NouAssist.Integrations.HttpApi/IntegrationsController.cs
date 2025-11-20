using NouAssist.Integrations.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Integrations;

public abstract class IntegrationsController : AbpControllerBase
{
    protected IntegrationsController()
    {
        LocalizationResource = typeof(IntegrationsResource);
    }
}
