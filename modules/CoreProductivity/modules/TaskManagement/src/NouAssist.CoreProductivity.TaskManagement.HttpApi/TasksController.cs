using NouAssist.CoreProductivity.TaskManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CoreProductivity.TaskManagement;

public abstract class TasksController : AbpControllerBase
{
    protected TasksController()
    {
        LocalizationResource = typeof(TaskManagementResource);
    }
}
