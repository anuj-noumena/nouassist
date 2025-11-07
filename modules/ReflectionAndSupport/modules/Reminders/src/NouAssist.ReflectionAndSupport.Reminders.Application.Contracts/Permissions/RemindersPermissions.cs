using Volo.Abp.Reflection;

namespace NouAssist.ReflectionAndSupport.Reminders.Permissions;

public class RemindersPermissions
{
    public const string GroupName = "Reminders";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(RemindersPermissions));
    }
}
