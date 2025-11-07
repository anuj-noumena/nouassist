using NouAssist.Reminders.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Reminders;

public abstract class RemindersController : AbpControllerBase
{
    protected RemindersController()
    {
        LocalizationResource = typeof(RemindersResource);
    }
}
