using NouAssist.Approval.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Approval.Permissions
{
    public class ApprovalPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var approvalGroup = context.AddGroup(
                ApprovalPermissions.GroupName,
                L("Permission:Approval"));

            var workflowsPermission = approvalGroup.AddPermission(
                ApprovalPermissions.Workflows.Default,
                L("Permission:Workflows"));
            workflowsPermission.AddChild(
                ApprovalPermissions.Workflows.Create,
                L("Permission:Workflows.Create"));
            workflowsPermission.AddChild(
                ApprovalPermissions.Workflows.Update,
                L("Permission:Workflows.Update"));
            workflowsPermission.AddChild(
                ApprovalPermissions.Workflows.Delete,
                L("Permission:Workflows.Delete"));
            workflowsPermission.AddChild(
                ApprovalPermissions.Workflows.Activate,
                L("Permission:Workflows.Activate"));
            workflowsPermission.AddChild(
                ApprovalPermissions.Workflows.Deactivate,
                L("Permission:Workflows.Deactivate"));

            var requestsPermission = approvalGroup.AddPermission(
                ApprovalPermissions.Requests.Default,
                L("Permission:Requests"));
            requestsPermission.AddChild(
                ApprovalPermissions.Requests.Create,
                L("Permission:Requests.Create"));
            requestsPermission.AddChild(
                ApprovalPermissions.Requests.View,
                L("Permission:Requests.View"));
            requestsPermission.AddChild(
                ApprovalPermissions.Requests.Approve,
                L("Permission:Requests.Approve"));
            requestsPermission.AddChild(
                ApprovalPermissions.Requests.Reject,
                L("Permission:Requests.Reject"));
            requestsPermission.AddChild(
                ApprovalPermissions.Requests.Cancel,
                L("Permission:Requests.Cancel"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ApprovalResource>(name);
        }
    }
}
