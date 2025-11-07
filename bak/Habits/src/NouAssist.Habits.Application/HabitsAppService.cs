using NouAssist.Habits.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Habits;

public abstract class HabitsAppService : ApplicationService
{
    protected HabitsAppService()
    {
        LocalizationResource = typeof(HabitsResource);
        ObjectMapperContext = typeof(HabitsApplicationModule);
    }
}
