using NouAssist.Goals.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Goals;

public abstract class GoalsController : AbpControllerBase
{
    protected GoalsController()
    {
        LocalizationResource = typeof(GoalsResource);
    }
}
