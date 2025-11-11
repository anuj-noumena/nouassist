using NouAssist.TimeManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.TimeManagement;

public abstract class TimeManagementAppService : ApplicationService
{
    protected TimeManagementAppService()
    {
        LocalizationResource = typeof(TimeManagementResource);
        ObjectMapperContext = typeof(TimeManagementApplicationModule);
    }
}
