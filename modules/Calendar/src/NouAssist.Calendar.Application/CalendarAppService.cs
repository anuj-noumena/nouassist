using NouAssist.Calendar.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Calendar;

public abstract class CalendarAppService : ApplicationService
{
    protected CalendarAppService()
    {
        LocalizationResource = typeof(CalendarResource);
        ObjectMapperContext = typeof(CalendarApplicationModule);
    }
}
