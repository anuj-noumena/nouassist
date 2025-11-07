using NouAssist.ReflectionAndSupport.ActivityTracking.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

public abstract class ActivityTrackingController : AbpControllerBase
{
    protected ActivityTrackingController()
    {
        LocalizationResource = typeof(ActivityTrackingResource);
    }
}
