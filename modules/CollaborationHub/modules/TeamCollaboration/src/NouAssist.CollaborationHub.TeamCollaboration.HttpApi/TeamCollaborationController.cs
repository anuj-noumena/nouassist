using NouAssist.CollaborationHub.TeamCollaboration.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CollaborationHub.TeamCollaboration;

public abstract class TeamCollaborationController : AbpControllerBase
{
    protected TeamCollaborationController()
    {
        LocalizationResource = typeof(TeamCollaborationResource);
    }
}
