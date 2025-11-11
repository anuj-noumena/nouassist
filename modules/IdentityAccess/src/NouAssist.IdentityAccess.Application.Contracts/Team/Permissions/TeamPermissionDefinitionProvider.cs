using NouAssist.Team.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Team.Permissions
{
    public class TeamPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var teamGroup = context.AddGroup(
                TeamPermissions.GroupName,
                L("Permission:Team"));

            var teamsPermission = teamGroup.AddPermission(
                TeamPermissions.Teams.Default,
                L("Permission:Teams"));
            teamsPermission.AddChild(
                TeamPermissions.Teams.Create,
                L("Permission:Teams.Create"));
            teamsPermission.AddChild(
                TeamPermissions.Teams.Update,
                L("Permission:Teams.Update"));
            teamsPermission.AddChild(
                TeamPermissions.Teams.Delete,
                L("Permission:Teams.Delete"));
            teamsPermission.AddChild(
                TeamPermissions.Teams.ManageMembers,
                L("Permission:Teams.ManageMembers"));

            var rolesPermission = teamGroup.AddPermission(
                TeamPermissions.Roles.Default,
                L("Permission:Roles"));
            rolesPermission.AddChild(
                TeamPermissions.Roles.Create,
                L("Permission:Roles.Create"));
            rolesPermission.AddChild(
                TeamPermissions.Roles.Update,
                L("Permission:Roles.Update"));
            rolesPermission.AddChild(
                TeamPermissions.Roles.Delete,
                L("Permission:Roles.Delete"));
            rolesPermission.AddChild(
                TeamPermissions.Roles.AssignToUsers,
                L("Permission:Roles.AssignToUsers"));

            var membersPermission = teamGroup.AddPermission(
                TeamPermissions.Members.Default,
                L("Permission:Members"));
            membersPermission.AddChild(
                TeamPermissions.Members.Add,
                L("Permission:Members.Add"));
            membersPermission.AddChild(
                TeamPermissions.Members.Remove,
                L("Permission:Members.Remove"));
            membersPermission.AddChild(
                TeamPermissions.Members.UpdateRole,
                L("Permission:Members.UpdateRole"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<TeamResource>(name);
        }
    }
}
