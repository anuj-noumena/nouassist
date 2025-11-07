using NouAssist.CoreProductivity.GoalTracking.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.GoalTracking;

public abstract class GoalTrackingAppService : ApplicationService
{
    protected GoalTrackingAppService()
    {
        LocalizationResource = typeof(GoalTrackingResource);
        ObjectMapperContext = typeof(GoalTrackingApplicationModule);
    }
}
