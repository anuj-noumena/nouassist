using NouAssist.Notification.Notifications.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Notification.Notifications;

public abstract class NotificationsAppService : ApplicationService
{
    protected NotificationsAppService()
    {
        LocalizationResource = typeof(NotificationsResource);
        ObjectMapperContext = typeof(NotificationsApplicationModule);
    }
}
