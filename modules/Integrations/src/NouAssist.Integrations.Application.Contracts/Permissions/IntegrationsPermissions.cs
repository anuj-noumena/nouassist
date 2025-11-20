using Volo.Abp.Reflection;

namespace NouAssist.Integrations.Permissions;

public class IntegrationsPermissions
{
    public const string GroupName = "Integrations";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(IntegrationsPermissions));
    }
}
