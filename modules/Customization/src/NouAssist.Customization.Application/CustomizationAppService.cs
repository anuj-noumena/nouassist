using NouAssist.Customization.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Customization;

public abstract class CustomizationAppService : ApplicationService
{
    protected CustomizationAppService()
    {
        LocalizationResource = typeof(CustomizationResource);
        ObjectMapperContext = typeof(CustomizationApplicationModule);
    }
}
