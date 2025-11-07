using Volo.Abp.Reflection;

namespace NouAssist.ReflectionAndSupport.Journaling.Permissions;

public class JournalingPermissions
{
    public const string GroupName = "Journaling";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(JournalingPermissions));
    }
}
