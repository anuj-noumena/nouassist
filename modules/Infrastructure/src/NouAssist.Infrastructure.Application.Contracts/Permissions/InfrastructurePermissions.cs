using Volo.Abp.Reflection;

namespace NouAssist.Infrastructure.Permissions;

public class InfrastructurePermissions
{
    public const string GroupName = "Infrastructure";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(InfrastructurePermissions));
    }
}
