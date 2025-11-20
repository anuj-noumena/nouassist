using Volo.Abp.Reflection;

namespace NouAssist.IntelligenceStrategy.Permissions;

public class IntelligenceStrategyPermissions
{
    public const string GroupName = "IntelligenceStrategy";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(IntelligenceStrategyPermissions));
    }
}
