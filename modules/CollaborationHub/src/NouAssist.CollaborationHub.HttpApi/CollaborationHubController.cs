using NouAssist.CollaborationHub.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CollaborationHub;

public abstract class CollaborationHubController : AbpControllerBase
{
    protected CollaborationHubController()
    {
        LocalizationResource = typeof(CollaborationHubResource);
    }
}
