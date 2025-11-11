using NouAssist.WorkManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement;

public abstract class WorkManagementAppService : ApplicationService
{
    protected WorkManagementAppService()
    {
        LocalizationResource = typeof(WorkManagementResource);
        ObjectMapperContext = typeof(WorkManagementApplicationModule);
    }
}
