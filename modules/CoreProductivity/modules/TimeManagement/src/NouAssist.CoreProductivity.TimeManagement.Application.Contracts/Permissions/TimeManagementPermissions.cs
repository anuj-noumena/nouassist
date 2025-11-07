using Volo.Abp.Reflection;

namespace NouAssist.CoreProductivity.TimeManagement.Permissions;

public class TimeManagementPermissions
{
    public const string GroupName = "TimeManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(TimeManagementPermissions));
    }
}
