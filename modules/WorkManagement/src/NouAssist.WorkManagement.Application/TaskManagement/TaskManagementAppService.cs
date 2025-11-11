using NouAssist.CoreProductivity.TaskManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.TaskManagement;

public abstract class TaskManagementAppService : ApplicationService
{
    protected TaskManagementAppService()
    {
        LocalizationResource = typeof(TaskManagementResource);
        ObjectMapperContext = typeof(TaskManagementApplicationModule);
    }
}
