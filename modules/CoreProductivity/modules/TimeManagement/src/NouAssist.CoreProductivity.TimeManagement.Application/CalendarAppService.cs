using NouAssist.CoreProductivity.TimeManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.TimeManagement;

public abstract class CalendarAppService : ApplicationService
{
    protected CalendarAppService()
    {
        LocalizationResource = typeof(TimeManagementResource);
        ObjectMapperContext = typeof(TimeManagementApplicationModule);
    }
}
