using NouAssist.Notifications.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Notifications;

public abstract class NotificationsAppService : ApplicationService
{
    protected NotificationsAppService()
    {
        LocalizationResource = typeof(NotificationsResource);
        ObjectMapperContext = typeof(NotificationsApplicationModule);
    }
}
