using Volo.Abp.Reflection;

namespace NouAssist.WorkflowOrchestration.Permissions;

public class WorkflowOrchestrationPermissions
{
    public const string GroupName = "WorkflowOrchestration";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(WorkflowOrchestrationPermissions));
    }
}
