using Volo.Abp.Reflection;

namespace NouAssist.Collaboration.Permissions;

public class CollaborationPermissions
{
    public const string GroupName = "Collaboration";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CollaborationPermissions));
    }
}
