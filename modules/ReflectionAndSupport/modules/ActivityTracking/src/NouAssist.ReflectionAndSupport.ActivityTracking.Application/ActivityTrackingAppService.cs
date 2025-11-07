using NouAssist.ReflectionAndSupport.ActivityTracking.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

public abstract class ActivityTrackingAppService : ApplicationService
{
    protected ActivityTrackingAppService()
    {
        LocalizationResource = typeof(ActivityTrackingResource);
        ObjectMapperContext = typeof(ActivityTrackingApplicationModule);
    }
}
