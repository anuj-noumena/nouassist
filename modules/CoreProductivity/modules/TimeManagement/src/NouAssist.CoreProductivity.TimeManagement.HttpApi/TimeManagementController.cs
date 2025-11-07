using NouAssist.CoreProductivity.TimeManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CoreProductivity.TimeManagement;

public abstract class TimeManagementController : AbpControllerBase
{
    protected TimeManagementController()
    {
        LocalizationResource = typeof(TimeManagementResource);
    }
}
