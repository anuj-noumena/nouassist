using NouAssist.CoreProductivity.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CoreProductivity;

public abstract class CoreProductivityController : AbpControllerBase
{
    protected CoreProductivityController()
    {
        LocalizationResource = typeof(CoreProductivityResource);
    }
}
