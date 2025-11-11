using NouAssist.WorkManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.WorkManagement;

public abstract class WorkManagementController : AbpControllerBase
{
    protected WorkManagementController()
    {
        LocalizationResource = typeof(WorkManagementResource);
    }
}
