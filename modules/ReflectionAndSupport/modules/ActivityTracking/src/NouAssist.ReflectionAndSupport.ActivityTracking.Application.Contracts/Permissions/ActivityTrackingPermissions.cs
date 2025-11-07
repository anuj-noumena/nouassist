using Volo.Abp.Reflection;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.Permissions;

public class ActivityTrackingPermissions
{
    public const string GroupName = "ActivityTracking";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ActivityTrackingPermissions));
    }
}
