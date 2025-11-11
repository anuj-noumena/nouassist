using Volo.Abp.Reflection;

namespace NouAssist.IdentityAccess.Permissions;

public class IdentityAccessPermissions
{
    public const string GroupName = "IdentityAccess";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityAccessPermissions));
    }
}
