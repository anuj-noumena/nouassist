using NouAssist.Resources.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Resources;

public abstract class ResourcesAppService : ApplicationService
{
    protected ResourcesAppService()
    {
        LocalizationResource = typeof(ResourcesResource);
        ObjectMapperContext = typeof(ResourcesApplicationModule);
    }
}
