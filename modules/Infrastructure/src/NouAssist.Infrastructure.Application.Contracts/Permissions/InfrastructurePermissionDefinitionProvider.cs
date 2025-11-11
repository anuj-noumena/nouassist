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
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<InfrastructureResource>(name);
    }
}
