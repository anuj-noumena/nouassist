using Volo.Abp.Application.Services;

namespace NouAssist.PersonalDevelopment.HabitSystems;

public abstract class HabitSystemsAppService : ApplicationService
{
    protected HabitSystemsAppService()
    {
        LocalizationResource = typeof(HabitSystemsResource);
        ObjectMapperContext = typeof(HabitSystemsApplicationModule);
    }
}
