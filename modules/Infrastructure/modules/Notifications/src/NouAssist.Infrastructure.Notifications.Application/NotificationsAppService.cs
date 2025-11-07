using NouAssist.Infrastructure.Notifications.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Infrastructure.Notifications;

public abstract class NotificationsAppService : ApplicationService
{
    protected NotificationsAppService()
    {
        LocalizationResource = typeof(NotificationsResource);
        ObjectMapperContext = typeof(NotificationsApplicationModule);
    }
}
