using NouAssist.CollaborationHub.TeamCollaboration.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CollaborationHub.TeamCollaboration;

public abstract class TeamController : AbpControllerBase
{
    protected TeamController()
    {
        LocalizationResource = typeof(TeamCollaborationResource);
    }
}
