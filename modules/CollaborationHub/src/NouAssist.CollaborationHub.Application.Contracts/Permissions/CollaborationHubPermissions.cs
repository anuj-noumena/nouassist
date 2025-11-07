using Volo.Abp.Reflection;

namespace NouAssist.CollaborationHub.Permissions;

public class CollaborationHubPermissions
{
    public const string GroupName = "CollaborationHub";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CollaborationHubPermissions));
    }
}
