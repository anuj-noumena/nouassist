using NouAssist.Notification.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Notification;

public abstract class NotificationController : AbpControllerBase
{
    protected NotificationController()
    {
        LocalizationResource = typeof(NotificationResource);
    }
}
