using NouAssist.Projects.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Projects;

public abstract class ProjectsController : AbpControllerBase
{
    protected ProjectsController()
    {
        LocalizationResource = typeof(ProjectsResource);
    }
}
