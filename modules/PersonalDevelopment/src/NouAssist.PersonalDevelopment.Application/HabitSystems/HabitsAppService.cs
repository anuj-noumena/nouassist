using NouAssist.CoreProductivity.HabitSystems.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.HabitSystems;

public abstract class HabitsAppService : ApplicationService
{
    protected HabitsAppService()
    {
        LocalizationResource = typeof(HabitSystemsResource);
        ObjectMapperContext = typeof(HabitSystemsApplicationModule);
    }
}
