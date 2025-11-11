using NouAssist.CoreProductivity.HabitSystems.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.HabitSystems;

public abstract class HabitSystemsAppService : ApplicationService
{
    protected HabitSystemsAppService()
    {
        LocalizationResource = typeof(HabitSystemsResource);
        ObjectMapperContext = typeof(HabitSystemsApplicationModule);
    }
}
