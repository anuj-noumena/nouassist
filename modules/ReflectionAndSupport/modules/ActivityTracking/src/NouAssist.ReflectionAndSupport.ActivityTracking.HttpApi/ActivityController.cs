using NouAssist.ReflectionAndSupport.ActivityTracking.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

public abstract class ActivityController : AbpControllerBase
{
    protected ActivityController()
    {
        LocalizationResource = typeof(ActivityTrackingResource);
    }
}
