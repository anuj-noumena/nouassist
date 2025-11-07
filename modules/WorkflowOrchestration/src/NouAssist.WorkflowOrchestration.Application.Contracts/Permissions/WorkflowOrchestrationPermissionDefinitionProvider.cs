using NouAssist.WorkflowOrchestration.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.WorkflowOrchestration.Permissions;

public class WorkflowOrchestrationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WorkflowOrchestrationPermissions.GroupName, L("Permission:WorkflowOrchestration"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WorkflowOrchestrationResource>(name);
    }
}
