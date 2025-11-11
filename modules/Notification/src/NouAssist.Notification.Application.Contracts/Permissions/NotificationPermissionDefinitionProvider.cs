using NouAssist.Notification.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Notification.Permissions;

public class NotificationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(NotificationPermissions.GroupName, L("Permission:Notification"));

        // Notifications Permissions
        var notificationsGroup = context.AddGroup(
            NotificationPermissions.Notifications.GroupName,
            L("Permission:Notifications"));

        var notificationsPermission = notificationsGroup.AddPermission(
            NotificationPermissions.Notifications.Notifications_.Default,
            L("Permission:Notifications"));
        notificationsPermission.AddChild(
            NotificationPermissions.Notifications.Notifications_.Manage,
            L("Permission:Notifications.Manage"));

        var preferencesPermission = notificationsGroup.AddPermission(
            NotificationPermissions.Notifications.Preferences.Default,
            L("Permission:Preferences"));
        preferencesPermission.AddChild(
            NotificationPermissions.Notifications.Preferences.Manage,
            L("Permission:Preferences.Manage"));

        // Reminders Permissions
        var remindersGroup = context.AddGroup(
            NotificationPermissions.Reminders.GroupName,
            L("Permission:Reminders"));

        var remindersPermission = remindersGroup.AddPermission(
            NotificationPermissions.Reminders.Reminders_.Default,
            L("Permission:Reminders"));
        remindersPermission.AddChild(
            NotificationPermissions.Reminders.Reminders_.Create,
            L("Permission:Reminders.Create"));
        remindersPermission.AddChild(
            NotificationPermissions.Reminders.Reminders_.Update,
            L("Permission:Reminders.Update"));
        remindersPermission.AddChild(
            NotificationPermissions.Reminders.Reminders_.Delete,
            L("Permission:Reminders.Delete"));
        remindersPermission.AddChild(
            NotificationPermissions.Reminders.Reminders_.Snooze,
            L("Permission:Reminders.Snooze"));
        remindersPermission.AddChild(
            NotificationPermissions.Reminders.Reminders_.ViewAll,
            L("Permission:Reminders.ViewAll"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NotificationResource>(name);
    }
}
