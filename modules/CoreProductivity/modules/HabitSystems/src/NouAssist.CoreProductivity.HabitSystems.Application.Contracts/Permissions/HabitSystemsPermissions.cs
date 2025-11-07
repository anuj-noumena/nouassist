using Volo.Abp.Reflection;

namespace NouAssist.CoreProductivity.HabitSystems.Permissions;

public class HabitSystemsPermissions
{
    public const string GroupName = "HabitSystems";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(HabitSystemsPermissions));
    }
}
