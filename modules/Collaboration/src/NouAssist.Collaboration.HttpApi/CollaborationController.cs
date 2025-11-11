using NouAssist.Collaboration.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Collaboration;

public abstract class CollaborationController : AbpControllerBase
{
    protected CollaborationController()
    {
        LocalizationResource = typeof(CollaborationResource);
    }
}
