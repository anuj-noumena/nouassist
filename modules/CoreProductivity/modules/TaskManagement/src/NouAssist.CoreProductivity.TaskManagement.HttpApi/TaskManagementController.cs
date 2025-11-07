using NouAssist.CoreProductivity.TaskManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CoreProductivity.TaskManagement;

public abstract class TaskManagementController : AbpControllerBase
{
    protected TaskManagementController()
    {
        LocalizationResource = typeof(TaskManagementResource);
    }
}
