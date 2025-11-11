using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.ProjectManagement;

public abstract class ProjectsAppService : ApplicationService
{
    protected ProjectsAppService()
    {
        LocalizationResource = typeof(ProjectManagementResource);
        ObjectMapperContext = typeof(ProjectManagementApplicationModule);
    }
}
