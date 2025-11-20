using Volo.Abp.Reflection;

namespace NouAssist.RequirementsManagement.Permissions;

public class RequirementsManagementPermissions
{
    public const string GroupName = "RequirementsManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(RequirementsManagementPermissions));
    }
}
