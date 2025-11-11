using Volo.Abp.Reflection;

namespace NouAssist.WorkManagement.GTD.Permissions;

public class GTDPermissions
{
    public const string GroupName = "GTD";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(GTDPermissions));
    }
}
