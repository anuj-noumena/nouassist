using Volo.Abp.Reflection;

namespace NouAssist.ReflectionAndSupport.Permissions;

public class ReflectionAndSupportPermissions
{
    public const string GroupName = "ReflectionAndSupport";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ReflectionAndSupportPermissions));
    }
}
