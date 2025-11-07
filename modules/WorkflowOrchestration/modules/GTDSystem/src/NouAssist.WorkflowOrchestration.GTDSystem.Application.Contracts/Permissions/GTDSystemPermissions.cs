using Volo.Abp.Reflection;

namespace NouAssist.WorkflowOrchestration.GTDSystem.Permissions;

public class GTDSystemPermissions
{
    public const string GroupName = "GTDSystem";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(GTDSystemPermissions));
    }
}
