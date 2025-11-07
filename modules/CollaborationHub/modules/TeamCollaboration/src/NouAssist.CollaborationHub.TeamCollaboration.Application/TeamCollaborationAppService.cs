using NouAssist.CollaborationHub.TeamCollaboration.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.TeamCollaboration;

public abstract class TeamCollaborationAppService : ApplicationService
{
    protected TeamCollaborationAppService()
    {
        LocalizationResource = typeof(TeamCollaborationResource);
        ObjectMapperContext = typeof(TeamCollaborationApplicationModule);
    }
}
