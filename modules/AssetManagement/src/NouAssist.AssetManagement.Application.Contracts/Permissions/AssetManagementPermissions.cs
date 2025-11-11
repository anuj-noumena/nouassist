using Volo.Abp.Reflection;

namespace NouAssist.AssetManagement.Permissions;

public class AssetManagementPermissions
{
    public const string GroupName = "AssetManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(AssetManagementPermissions));
    }
}
