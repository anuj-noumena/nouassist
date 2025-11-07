using NouAssist.CoreProductivity.GoalTracking.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.GoalTracking;

public abstract class GoalsAppService : ApplicationService
{
    protected GoalsAppService()
    {
        LocalizationResource = typeof(GoalTrackingResource);
        ObjectMapperContext = typeof(GoalTrackingApplicationModule);
    }
}
