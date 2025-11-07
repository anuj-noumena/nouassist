using NouAssist.CollaborationHub.TeamCollaboration.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.TeamCollaboration;

public abstract class MeetingsAppService : ApplicationService
{
    protected MeetingsAppService()
    {
        LocalizationResource = typeof(TeamCollaborationResource);
        ObjectMapperContext = typeof(TeamCollaborationApplicationModule);
    }
}
