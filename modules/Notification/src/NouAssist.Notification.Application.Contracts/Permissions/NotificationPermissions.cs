using Volo.Abp.Reflection;

namespace NouAssist.Notification.Permissions;

public class NotificationPermissions
{
    public const string GroupName = "Notification";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(NotificationPermissions));
    }
}
