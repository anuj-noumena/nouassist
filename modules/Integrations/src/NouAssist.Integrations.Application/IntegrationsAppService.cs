using NouAssist.Integrations.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Integrations;

public abstract class IntegrationsAppService : ApplicationService
{
    protected IntegrationsAppService()
    {
        LocalizationResource = typeof(IntegrationsResource);
        ObjectMapperContext = typeof(IntegrationsApplicationModule);
    }
}
