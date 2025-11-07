using NouAssist.CollaborationHub.TeamCollaboration.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CollaborationHub.TeamCollaboration;

public abstract class MeetingsController : AbpControllerBase
{
    protected MeetingsController()
    {
        LocalizationResource = typeof(TeamCollaborationResource);
    }
}
