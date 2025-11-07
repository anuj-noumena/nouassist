using NouAssist.Notifications.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Notifications;

public abstract class NotificationsController : AbpControllerBase
{
    protected NotificationsController()
    {
        LocalizationResource = typeof(NotificationsResource);
    }
}
