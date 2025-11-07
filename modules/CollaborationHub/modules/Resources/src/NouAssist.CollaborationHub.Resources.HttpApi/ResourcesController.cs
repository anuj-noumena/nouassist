using NouAssist.CollaborationHub.Resources.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CollaborationHub.Resources;

public abstract class ResourcesController : AbpControllerBase
{
    protected ResourcesController()
    {
        LocalizationResource = typeof(ResourcesResource);
    }
}
