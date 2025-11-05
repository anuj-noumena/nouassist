using NouAssist.Activity.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Activity;

public abstract class ActivityAppService : ApplicationService
{
    protected ActivityAppService()
    {
        LocalizationResource = typeof(ActivityResource);
        ObjectMapperContext = typeof(ActivityApplicationModule);
    }
}
