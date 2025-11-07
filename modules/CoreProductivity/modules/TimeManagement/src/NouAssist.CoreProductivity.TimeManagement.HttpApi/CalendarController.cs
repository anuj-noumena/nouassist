using NouAssist.CoreProductivity.TimeManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CoreProductivity.TimeManagement;

public abstract class CalendarController : AbpControllerBase
{
    protected CalendarController()
    {
        LocalizationResource = typeof(TimeManagementResource);
    }
}
