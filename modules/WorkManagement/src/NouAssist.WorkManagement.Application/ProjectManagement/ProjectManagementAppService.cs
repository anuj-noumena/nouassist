using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.ProjectManagement;

public abstract class ProjectManagementAppService : ApplicationService
{
    protected ProjectManagementAppService()
    {
        LocalizationResource = typeof(ProjectManagementResource);
        ObjectMapperContext = typeof(ProjectManagementApplicationModule);
    }
}
