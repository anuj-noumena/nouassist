using NouAssist.Infrastructure.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Infrastructure.Permissions;

public class InfrastructurePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var infrastructureGroup = context.AddGroup(InfrastructurePermissions.GroupName, L("Permission:Infrastructure"));

        // Activity Permissions
        var activityFeedsPermission = infrastructureGroup.AddPermission(
            InfrastructurePermissions.Activity.ActivityFeeds.Default,
            L("Permission:Activity.ActivityFeeds"));
        activityFeedsPermission.AddChild(
            InfrastructurePermissions.Activity.ActivityFeeds.ViewAll,
            L("Permission:Activity.ActivityFeeds.ViewAll"));
        activityFeedsPermission.AddChild(
            InfrastructurePermissions.Activity.ActivityFeeds.Manage,
            L("Permission:Activity.ActivityFeeds.Manage"));

        var auditLogsPermission = infrastructureGroup.AddPermission(
            InfrastructurePermissions.Activity.AuditLogs.Default,
            L("Permission:Activity.AuditLogs"));
        auditLogsPermission.AddChild(
            InfrastructurePermissions.Activity.AuditLogs.View,
            L("Permission:Activity.AuditLogs.View"));
        auditLogsPermission.AddChild(
            InfrastructurePermissions.Activity.AuditLogs.ViewAll,
            L("Permission:Activity.AuditLogs.ViewAll"));
        auditLogsPermission.AddChild(
            InfrastructurePermissions.Activity.AuditLogs.Export,
            L("Permission:Activity.AuditLogs.Export"));

        var entityHistoryPermission = infrastructureGroup.AddPermission(
            InfrastructurePermissions.Activity.EntityHistory.Default,
            L("Permission:Activity.EntityHistory"));
        entityHistoryPermission.AddChild(
            InfrastructurePermissions.Activity.EntityHistory.View,
            L("Permission:Activity.EntityHistory.View"));

        // Search Permissions
        var searchPermission = infrastructureGroup.AddPermission(
            InfrastructurePermissions.Search.SearchFeatures.Default,
            L("Permission:Search"));
        searchPermission.AddChild(
            InfrastructurePermissions.Search.SearchFeatures.SaveSearches,
            L("Permission:Search.SaveSearches"));
        searchPermission.AddChild(
            InfrastructurePermissions.Search.SearchFeatures.ViewHistory,
            L("Permission:Search.ViewHistory"));
        searchPermission.AddChild(
            InfrastructurePermissions.Search.SearchFeatures.ViewAnalytics,
            L("Permission:Search.ViewAnalytics"));
        
        
        // Notifications Permissions
        var notificationsGroup = context.AddGroup(
            InfrastructurePermissions.Notifications.GroupName,
            L("Permission:Notifications"));

        var notificationsPermission = notificationsGroup.AddPermission(
            InfrastructurePermissions.Notifications.Notifications_.Default,
            L("Permission:Notifications"));
        notificationsPermission.AddChild(
            InfrastructurePermissions.Notifications.Notifications_.Manage,
            L("Permission:Notifications.Manage"));

        var preferencesPermission = notificationsGroup.AddPermission(
            InfrastructurePermissions.Notifications.Preferences.Default,
            L("Permission:Preferences"));
        preferencesPermission.AddChild(
            InfrastructurePermissions.Notifications.Preferences.Manage,
            L("Permission:Preferences.Manage"));

        // Reminders Permissions
        var remindersGroup = context.AddGroup(
            InfrastructurePermissions.Reminders.GroupName,
            L("Permission:Reminders"));

        var remindersPermission = remindersGroup.AddPermission(
            InfrastructurePermissions.Reminders.Reminders_.Default,
            L("Permission:Reminders"));
        remindersPermission.AddChild(
            InfrastructurePermissions.Reminders.Reminders_.Create,
            L("Permission:Reminders.Create"));
        remindersPermission.AddChild(
            InfrastructurePermissions.Reminders.Reminders_.Update,
            L("Permission:Reminders.Update"));
        remindersPermission.AddChild(
            InfrastructurePermissions.Reminders.Reminders_.Delete,
            L("Permission:Reminders.Delete"));
        remindersPermission.AddChild(
            InfrastructurePermissions.Reminders.Reminders_.Snooze,
            L("Permission:Reminders.Snooze"));
        remindersPermission.AddChild(
            InfrastructurePermissions.Reminders.Reminders_.ViewAll,
            L("Permission:Reminders.ViewAll"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<InfrastructureResource>(name);
    }
}
