using Volo.Abp.Reflection;

namespace NouAssist.Workflow.Permissions;

public class WorkflowPermissions
{
    public const string GroupName = "Workflow";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(WorkflowPermissions));
    }
}
