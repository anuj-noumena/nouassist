using NouAssist.CollaborationHub.TeamCollaboration.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CollaborationHub.TeamCollaboration.Permissions;

public class TeamCollaborationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(TeamCollaborationPermissions.GroupName, L("Permission:TeamCollaboration"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<TeamCollaborationResource>(name);
    }
}
