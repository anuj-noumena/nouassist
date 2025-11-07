using NouAssist.ReflectionAndSupport.Reminders.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.Reminders;

public abstract class RemindersAppService : ApplicationService
{
    protected RemindersAppService()
    {
        LocalizationResource = typeof(RemindersResource);
        ObjectMapperContext = typeof(RemindersApplicationModule);
    }
}
