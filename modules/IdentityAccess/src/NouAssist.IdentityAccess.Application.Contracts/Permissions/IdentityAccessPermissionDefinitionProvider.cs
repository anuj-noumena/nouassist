using NouAssist.IdentityAccess.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.IdentityAccess.Permissions;

public class IdentityAccessPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(IdentityAccessPermissions.GroupName, L("Permission:IdentityAccess"));

        // Team Permissions
        var teamGroup = context.AddGroup(
            IdentityAccessPermissions.Team.GroupName,
            L("Permission:Team"));

        var teamsPermission = teamGroup.AddPermission(
            IdentityAccessPermissions.Team.Teams.Default,
            L("Permission:Teams"));
        teamsPermission.AddChild(
            IdentityAccessPermissions.Team.Teams.Create,
            L("Permission:Teams.Create"));
        teamsPermission.AddChild(
            IdentityAccessPermissions.Team.Teams.Update,
            L("Permission:Teams.Update"));
        teamsPermission.AddChild(
            IdentityAccessPermissions.Team.Teams.Delete,
            L("Permission:Teams.Delete"));
        teamsPermission.AddChild(
            IdentityAccessPermissions.Team.Teams.ManageMembers,
            L("Permission:Teams.ManageMembers"));

        var rolesPermission = teamGroup.AddPermission(
            IdentityAccessPermissions.Team.Roles.Default,
            L("Permission:Roles"));
        rolesPermission.AddChild(
            IdentityAccessPermissions.Team.Roles.Create,
            L("Permission:Roles.Create"));
        rolesPermission.AddChild(
            IdentityAccessPermissions.Team.Roles.Update,
            L("Permission:Roles.Update"));
        rolesPermission.AddChild(
            IdentityAccessPermissions.Team.Roles.Delete,
            L("Permission:Roles.Delete"));
        rolesPermission.AddChild(
            IdentityAccessPermissions.Team.Roles.AssignToUsers,
            L("Permission:Roles.AssignToUsers"));

        var membersPermission = teamGroup.AddPermission(
            IdentityAccessPermissions.Team.Members.Default,
            L("Permission:Members"));
        membersPermission.AddChild(
            IdentityAccessPermissions.Team.Members.Add,
            L("Permission:Members.Add"));
        membersPermission.AddChild(
            IdentityAccessPermissions.Team.Members.Remove,
            L("Permission:Members.Remove"));
        membersPermission.AddChild(
            IdentityAccessPermissions.Team.Members.UpdateRole,
            L("Permission:Members.UpdateRole"));

        // Users Permissions
        var usersGroup = context.AddGroup(
            IdentityAccessPermissions.Users.GroupName,
            L("Permission:Users"));

        var usersPermission = usersGroup.AddPermission(
            IdentityAccessPermissions.Users.UserManagement.Default,
            L("Permission:Users"));

        usersPermission.AddChild(
            IdentityAccessPermissions.Users.UserManagement.View,
            L("Permission:Users.View"));

        usersPermission.AddChild(
            IdentityAccessPermissions.Users.UserManagement.Update,
            L("Permission:Users.Update"));

        usersPermission.AddChild(
            IdentityAccessPermissions.Users.UserManagement.UpdateProfile,
            L("Permission:Users.UpdateProfile"));

        usersPermission.AddChild(
            IdentityAccessPermissions.Users.UserManagement.ManageSettings,
            L("Permission:Users.ManageSettings"));

        // Contacts Permissions
        var contactsGroup = context.AddGroup(
            IdentityAccessPermissions.Contacts.GroupName,
            L("Permission:Contacts"));

        var contactsPermission = contactsGroup.AddPermission(
            IdentityAccessPermissions.Contacts.ContactManagement.Default,
            L("Permission:Contacts"));

        contactsPermission.AddChild(
            IdentityAccessPermissions.Contacts.ContactManagement.Create,
            L("Permission:Contacts.Create"));

        contactsPermission.AddChild(
            IdentityAccessPermissions.Contacts.ContactManagement.Edit,
            L("Permission:Contacts.Edit"));

        contactsPermission.AddChild(
            IdentityAccessPermissions.Contacts.ContactManagement.Delete,
            L("Permission:Contacts.Delete"));

        contactsPermission.AddChild(
            IdentityAccessPermissions.Contacts.ContactManagement.Archive,
            L("Permission:Contacts.Archive"));

        var clientsPermission = contactsGroup.AddPermission(
            IdentityAccessPermissions.Contacts.Clients.Default,
            L("Permission:Clients"));

        clientsPermission.AddChild(
            IdentityAccessPermissions.Contacts.Clients.Create,
            L("Permission:Clients.Create"));

        clientsPermission.AddChild(
            IdentityAccessPermissions.Contacts.Clients.Edit,
            L("Permission:Clients.Edit"));

        clientsPermission.AddChild(
            IdentityAccessPermissions.Contacts.Clients.Delete,
            L("Permission:Clients.Delete"));

        clientsPermission.AddChild(
            IdentityAccessPermissions.Contacts.Clients.ManageStatus,
            L("Permission:Clients.ManageStatus"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityAccessResource>(name);
    }
}
