using NouAssist.CollaborationHub.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub;

public abstract class CollaborationHubAppService : ApplicationService
{
    protected CollaborationHubAppService()
    {
        LocalizationResource = typeof(CollaborationHubResource);
        ObjectMapperContext = typeof(CollaborationHubApplicationModule);
    }
}
