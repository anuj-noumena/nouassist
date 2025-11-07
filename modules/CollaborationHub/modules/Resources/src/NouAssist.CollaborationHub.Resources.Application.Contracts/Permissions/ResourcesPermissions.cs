using Volo.Abp.Reflection;

namespace NouAssist.CollaborationHub.Resources.Permissions;

public class ResourcesPermissions
{
    public const string GroupName = "Resources";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ResourcesPermissions));
    }
}
