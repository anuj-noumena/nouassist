using NouAssist.Resources.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Resources;

public abstract class ResourcesController : AbpControllerBase
{
    protected ResourcesController()
    {
        LocalizationResource = typeof(ResourcesResource);
    }
}
