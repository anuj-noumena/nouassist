using Volo.Abp.Application.Services;

namespace NouAssist.PersonalDevelopment.HabitSystems;

public abstract class HabitsAppService : ApplicationService
{
    protected HabitsAppService()
    {
        LocalizationResource = typeof(HabitSystemsResource);
        ObjectMapperContext = typeof(HabitSystemsApplicationModule);
    }
}
