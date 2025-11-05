using NouAssist.Tasks.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Tasks;

public abstract class TasksController : AbpControllerBase
{
    protected TasksController()
    {
        LocalizationResource = typeof(TasksResource);
    }
}
