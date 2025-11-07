using NouAssist.ReflectionAndSupport.Reminders.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.ReflectionAndSupport.Reminders;

public abstract class RemindersController : AbpControllerBase
{
    protected RemindersController()
    {
        LocalizationResource = typeof(RemindersResource);
    }
}
