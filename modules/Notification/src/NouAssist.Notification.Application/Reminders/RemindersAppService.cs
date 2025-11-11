using NouAssist.Notification.Reminders.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Notification.Reminders;

public abstract class RemindersAppService : ApplicationService
{
    protected RemindersAppService()
    {
        LocalizationResource = typeof(RemindersResource);
        ObjectMapperContext = typeof(RemindersApplicationModule);
    }
}
