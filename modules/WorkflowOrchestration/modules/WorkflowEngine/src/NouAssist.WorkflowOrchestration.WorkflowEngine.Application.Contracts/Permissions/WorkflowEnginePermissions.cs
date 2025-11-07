using Volo.Abp.Reflection;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Permissions;

public class WorkflowEnginePermissions
{
    public const string GroupName = "WorkflowEngine";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(WorkflowEnginePermissions));
    }
}
