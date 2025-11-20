using NouAssist.RequirementsManagement.ChangeManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.RequirementsManagement.ChangeManagement.Permissions
{
    public class ChangeManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            // Define a top-level group for the module
            var moduleGroup = context.AddGroup(ChangeManagementPermissions.GroupName, L("Permission:ChangeManagement"));

            // Define permissions for the ChangeRequest
            var changeRequestPermissionGroup = moduleGroup.AddPermission(
                ChangeManagementPermissions.ChangeRequest.Default, L("Permission:ChangeRequest"));

            // Add child permissions for specific actions
            changeRequestPermissionGroup.AddChild(
                ChangeManagementPermissions.ChangeRequest.Create, L("Permission:ChangeRequest.Create"));
            changeRequestPermissionGroup.AddChild(
                ChangeManagementPermissions.ChangeRequest.Update, L("Permission:ChangeRequest.Edit"));
            changeRequestPermissionGroup.AddChild(
                ChangeManagementPermissions.ChangeRequest.Delete, L("Permission:ChangeRequest.Delete"));
            changeRequestPermissionGroup.AddChild(
                ChangeManagementPermissions.ChangeRequest.View, L("Permission:ChangeRequest.View"));
            changeRequestPermissionGroup.AddChild(
                ChangeManagementPermissions.ChangeRequest.Approve, L("Permission:ChangeRequest.Approve"));
            changeRequestPermissionGroup.AddChild(
                ChangeManagementPermissions.ChangeRequest.Reject, L("Permission:ChangeRequest.Reject"));
            changeRequestPermissionGroup.AddChild(
                ChangeManagementPermissions.ChangeRequest.AssessImpact, L("Permission:ChangeRequest.AssessImpact"));
        }

        // Helper to create LocalizableString using the module's resource file marker
        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ChangeManagementResource>(name);
        }
    }
}
