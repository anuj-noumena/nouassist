using NouAssist.WorkManagement.TaskManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.TaskManagement;

public abstract class TasksAppService : ApplicationService
{
    protected TasksAppService()
    {
        LocalizationResource = typeof(TaskManagementResource);
        ObjectMapperContext = typeof(TaskManagementApplicationModule);
    }
}
