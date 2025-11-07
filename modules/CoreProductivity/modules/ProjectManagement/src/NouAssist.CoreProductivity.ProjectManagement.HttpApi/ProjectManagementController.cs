using NouAssist.CoreProductivity.ProjectManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CoreProductivity.ProjectManagement;

public abstract class ProjectManagementController : AbpControllerBase
{
    protected ProjectManagementController()
    {
        LocalizationResource = typeof(ProjectManagementResource);
    }
}
