using NouAssist.Habits.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Habits;

public abstract class HabitsController : AbpControllerBase
{
    protected HabitsController()
    {
        LocalizationResource = typeof(HabitsResource);
    }
}
