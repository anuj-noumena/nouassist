using Volo.Abp.Reflection;

namespace NouAssist.WorkManagement.Permissions;

public class WorkManagementPermissions
{
    public const string GroupName = "WorkManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(WorkManagementPermissions));
    }
}
