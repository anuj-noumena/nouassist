using NouAssist.Tasks.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Tasks;

public abstract class TasksAppService : ApplicationService
{
    protected TasksAppService()
    {
        LocalizationResource = typeof(TasksResource);
        ObjectMapperContext = typeof(TasksApplicationModule);
    }
}
