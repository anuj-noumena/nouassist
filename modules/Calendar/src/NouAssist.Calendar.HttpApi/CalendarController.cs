using NouAssist.Calendar.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Calendar;

public abstract class CalendarController : AbpControllerBase
{
    protected CalendarController()
    {
        LocalizationResource = typeof(CalendarResource);
    }
}
