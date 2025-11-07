using NouAssist.CoreProductivity.TaskManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.TaskManagement;

public abstract class TasksAppService : ApplicationService
{
    protected TasksAppService()
    {
        LocalizationResource = typeof(TaskManagementResource);
        ObjectMapperContext = typeof(TaskManagementApplicationModule);
    }
}
