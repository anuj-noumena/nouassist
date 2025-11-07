using NouAssist.CoreProductivity.HabitSystems.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CoreProductivity.HabitSystems;

public abstract class HabitsController : AbpControllerBase
{
    protected HabitsController()
    {
        LocalizationResource = typeof(HabitSystemsResource);
    }
}
