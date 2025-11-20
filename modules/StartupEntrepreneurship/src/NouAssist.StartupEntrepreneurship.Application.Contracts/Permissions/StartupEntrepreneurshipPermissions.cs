using Volo.Abp.Reflection;

namespace NouAssist.StartupEntrepreneurship.Permissions;

public class StartupEntrepreneurshipPermissions
{
    public const string GroupName = "StartupEntrepreneurship";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(StartupEntrepreneurshipPermissions));
    }
}
