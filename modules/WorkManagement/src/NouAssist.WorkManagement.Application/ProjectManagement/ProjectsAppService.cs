using NouAssist.CoreProductivity.ProjectManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.ProjectManagement;

public abstract class ProjectsAppService : ApplicationService
{
    protected ProjectsAppService()
    {
        LocalizationResource = typeof(ProjectManagementResource);
        ObjectMapperContext = typeof(ProjectManagementApplicationModule);
    }
}
