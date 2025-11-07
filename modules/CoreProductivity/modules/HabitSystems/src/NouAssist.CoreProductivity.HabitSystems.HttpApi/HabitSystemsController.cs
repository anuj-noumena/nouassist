using NouAssist.CoreProductivity.HabitSystems.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CoreProductivity.HabitSystems;

public abstract class HabitSystemsController : AbpControllerBase
{
    protected HabitSystemsController()
    {
        LocalizationResource = typeof(HabitSystemsResource);
    }
}
