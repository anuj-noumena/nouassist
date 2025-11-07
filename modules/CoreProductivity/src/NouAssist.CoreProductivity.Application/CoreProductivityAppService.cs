using NouAssist.CoreProductivity.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity;

public abstract class CoreProductivityAppService : ApplicationService
{
    protected CoreProductivityAppService()
    {
        LocalizationResource = typeof(CoreProductivityResource);
        ObjectMapperContext = typeof(CoreProductivityApplicationModule);
    }
}
