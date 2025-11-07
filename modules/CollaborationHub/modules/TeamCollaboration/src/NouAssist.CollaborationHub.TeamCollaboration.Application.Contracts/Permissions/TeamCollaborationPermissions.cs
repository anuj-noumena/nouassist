using Volo.Abp.Reflection;

namespace NouAssist.CollaborationHub.TeamCollaboration.Permissions;

public class TeamCollaborationPermissions
{
    public const string GroupName = "TeamCollaboration";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(TeamCollaborationPermissions));
    }
}
