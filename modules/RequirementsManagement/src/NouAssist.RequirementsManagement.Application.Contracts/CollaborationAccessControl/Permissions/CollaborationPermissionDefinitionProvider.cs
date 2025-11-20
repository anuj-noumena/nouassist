using NouAssist.RequirementsManagement.CollaborationAccessControl.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Permissions
{
    public class CollaborationPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var collaborationGroup = context.AddGroup(
                CollaborationPermissions.GroupName,
                L("Permission:Collaboration"));

            // Access Control permissions
            var accessControlPermission = collaborationGroup.AddPermission(
                CollaborationPermissions.AccessControl.Default,
                L("Permission:AccessControl"));

            accessControlPermission.AddChild(
                CollaborationPermissions.AccessControl.ManageRoles,
                L("Permission:ManageRoles"));

            accessControlPermission.AddChild(
                CollaborationPermissions.AccessControl.AssignRoles,
                L("Permission:AssignRoles"));

            accessControlPermission.AddChild(
                CollaborationPermissions.AccessControl.ManagePermissions,
                L("Permission:ManagePermissions"));

            accessControlPermission.AddChild(
                CollaborationPermissions.AccessControl.ViewPermissions,
                L("Permission:ViewPermissions"));

            // Notification permissions
            var notificationPermission = collaborationGroup.AddPermission(
                CollaborationPermissions.Notifications.Default,
                L("Permission:Notifications"));

            notificationPermission.AddChild(
                CollaborationPermissions.Notifications.Configure,
                L("Permission:ConfigureNotifications"));

            notificationPermission.AddChild(
                CollaborationPermissions.Notifications.SubscribeToEvents,
                L("Permission:SubscribeToEvents"));

            // Discussion permissions
            var discussionPermission = collaborationGroup.AddPermission(
                CollaborationPermissions.Discussions.Default,
                L("Permission:Discussions"));

            discussionPermission.AddChild(
                CollaborationPermissions.Discussions.CreateThreads,
                L("Permission:CreateThreads"));

            discussionPermission.AddChild(
                CollaborationPermissions.Discussions.AddComments,
                L("Permission:AddComments"));

            discussionPermission.AddChild(
                CollaborationPermissions.Discussions.ManageThreads,
                L("Permission:ManageThreads"));

            discussionPermission.AddChild(
                CollaborationPermissions.Discussions.ManageComments,
                L("Permission:ManageComments"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CollaborationResource>(name);
        }
    }
}
