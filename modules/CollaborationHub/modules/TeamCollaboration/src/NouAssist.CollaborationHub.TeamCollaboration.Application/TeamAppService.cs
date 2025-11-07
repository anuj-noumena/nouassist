using NouAssist.CollaborationHub.TeamCollaboration.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.TeamCollaboration;

public abstract class TeamAppService : ApplicationService
{
    protected TeamAppService()
    {
        LocalizationResource = typeof(TeamCollaborationResource);
        ObjectMapperContext = typeof(TeamCollaborationApplicationModule);
    }
}
