using NouAssist.CoreProductivity.GoalTracking.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CoreProductivity.GoalTracking;

public abstract class GoalsController : AbpControllerBase
{
    protected GoalsController()
    {
        LocalizationResource = typeof(GoalTrackingResource);
    }
}
