using NouAssist.WorkManagement.TaskManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.TaskManagement;

public abstract class TaskManagementAppService : ApplicationService
{
    protected TaskManagementAppService()
    {
        LocalizationResource = typeof(TaskManagementResource);
        ObjectMapperContext = typeof(TaskManagementApplicationModule);
    }
}
