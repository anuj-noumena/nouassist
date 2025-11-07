using NouAssist.CoreProductivity.GoalTracking.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CoreProductivity.GoalTracking;

public abstract class GoalTrackingController : AbpControllerBase
{
    protected GoalTrackingController()
    {
        LocalizationResource = typeof(GoalTrackingResource);
    }
}
