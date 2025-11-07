using NouAssist.Activity.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Activity;

public abstract class ActivityController : AbpControllerBase
{
    protected ActivityController()
    {
        LocalizationResource = typeof(ActivityResource);
    }
}
