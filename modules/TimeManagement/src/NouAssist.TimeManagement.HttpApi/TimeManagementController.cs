using NouAssist.TimeManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.TimeManagement;

public abstract class TimeManagementController : AbpControllerBase
{
    protected TimeManagementController()
    {
        LocalizationResource = typeof(TimeManagementResource);
    }
}
