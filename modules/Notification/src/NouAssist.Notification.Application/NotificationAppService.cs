using NouAssist.Notification.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Notification;

public abstract class NotificationAppService : ApplicationService
{
    protected NotificationAppService()
    {
        LocalizationResource = typeof(NotificationResource);
        ObjectMapperContext = typeof(NotificationApplicationModule);
    }
}
