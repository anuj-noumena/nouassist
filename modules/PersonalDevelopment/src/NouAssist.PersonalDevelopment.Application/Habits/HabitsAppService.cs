using NouAssist.PersonalDevelopment.Habits.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.PersonalDevelopment.Habits;

public abstract class HabitsAppService : ApplicationService
{
    protected HabitsAppService()
    {
        LocalizationResource = typeof(HabitsResource);
        ObjectMapperContext = typeof(HabitsApplicationModule);
    }
}
