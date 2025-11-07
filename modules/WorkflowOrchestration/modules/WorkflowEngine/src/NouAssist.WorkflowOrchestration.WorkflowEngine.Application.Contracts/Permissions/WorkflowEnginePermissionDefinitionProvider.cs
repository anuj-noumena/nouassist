using NouAssist.WorkflowOrchestration.WorkflowEngine.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Permissions;

public class WorkflowEnginePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WorkflowEnginePermissions.GroupName, L("Permission:WorkflowEngine"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WorkflowEngineResource>(name);
    }
}
