using NouAssist.CoreProductivity.ProjectManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.ProjectManagement;

public abstract class ProjectManagementAppService : ApplicationService
{
    protected ProjectManagementAppService()
    {
        LocalizationResource = typeof(ProjectManagementResource);
        ObjectMapperContext = typeof(ProjectManagementApplicationModule);
    }
}
