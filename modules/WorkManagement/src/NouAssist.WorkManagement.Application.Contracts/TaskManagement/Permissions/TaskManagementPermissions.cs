using Volo.Abp.Reflection;

namespace NouAssist.WorkManagement.TaskManagement.Permissions;

public class TaskManagementPermissions
{
    public const string GroupName = "TaskManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(TaskManagementPermissions));
    }
}
