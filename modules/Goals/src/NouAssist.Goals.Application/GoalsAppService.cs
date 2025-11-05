using NouAssist.Goals.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Goals;

public abstract class GoalsAppService : ApplicationService
{
    protected GoalsAppService()
    {
        LocalizationResource = typeof(GoalsResource);
        ObjectMapperContext = typeof(GoalsApplicationModule);
    }
}
