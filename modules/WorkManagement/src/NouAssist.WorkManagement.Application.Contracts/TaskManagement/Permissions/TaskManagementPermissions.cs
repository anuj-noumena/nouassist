using Volo.Abp.Reflection;

namespace NouAssist.CoreProductivity.TaskManagement.Permissions;

public class TaskManagementPermissions
{
    public const string GroupName = "TaskManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(TaskManagementPermissions));
    }
}
