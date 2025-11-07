using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.Permissions;

public class ApprovalWorkflowsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ApprovalWorkflowsPermissions.GroupName, L("Permission:ApprovalWorkflows"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ApprovalWorkflowsResource>(name);
    }
}
