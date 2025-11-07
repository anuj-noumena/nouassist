using NouAssist.Infrastructure.Notifications.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Infrastructure.Notifications;

public abstract class NotificationsController : AbpControllerBase
{
    protected NotificationsController()
    {
        LocalizationResource = typeof(NotificationsResource);
    }
}
