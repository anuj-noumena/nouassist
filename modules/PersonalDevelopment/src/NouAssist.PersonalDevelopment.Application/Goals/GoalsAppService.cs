using NouAssist.PersonalDevelopment.Goals.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.PersonalDevelopment.Goals;

public abstract class GoalsAppService : ApplicationService
{
    protected GoalsAppService()
    {
        LocalizationResource = typeof(GoalsResource);
        ObjectMapperContext = typeof(GoalsApplicationModule);
    }
}
