using Volo.Abp.Reflection;

namespace NouAssist.CoreProductivity.GoalTracking.Permissions;

public class GoalTrackingPermissions
{
    public const string GroupName = "GoalTracking";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(GoalTrackingPermissions));
    }
}
