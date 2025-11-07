using Volo.Abp.Reflection;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.Permissions;

public class ApprovalWorkflowsPermissions
{
    public const string GroupName = "ApprovalWorkflows";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ApprovalWorkflowsPermissions));
    }
}
