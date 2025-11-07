using Volo.Abp.Reflection;

namespace NouAssist.CoreProductivity.Permissions;

public class CoreProductivityPermissions
{
    public const string GroupName = "CoreProductivity";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CoreProductivityPermissions));
    }
}
