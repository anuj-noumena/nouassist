using NouAssist.IntelligenceStrategy.AnalyticsReporting.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Permissions
{
    /// <summary>
    /// Permission definition provider for the Analytics & Reporting module
    /// </summary>
    public class AnalyticsReportingPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            // Define the main module group
            var analyticsReportingGroup = context.AddGroup(
                AnalyticsReportingPermissions.GroupName,
                L("Permission:AnalyticsReporting")
            );

            // Dashboard permissions
            var dashboardPermission = analyticsReportingGroup.AddPermission(
                AnalyticsReportingPermissions.Dashboard.Default,
                L("Permission:Dashboard")
            );
            dashboardPermission.AddChild(
                AnalyticsReportingPermissions.Dashboard.Create,
                L("Permission:Dashboard.Create")
            );
            dashboardPermission.AddChild(
                AnalyticsReportingPermissions.Dashboard.Update,
                L("Permission:Dashboard.Update")
            );
            dashboardPermission.AddChild(
                AnalyticsReportingPermissions.Dashboard.Delete,
                L("Permission:Dashboard.Delete")
            );
            dashboardPermission.AddChild(
                AnalyticsReportingPermissions.Dashboard.View,
                L("Permission:Dashboard.View")
            );
            dashboardPermission.AddChild(
                AnalyticsReportingPermissions.Dashboard.AddWidget,
                L("Permission:Dashboard.AddWidget")
            );

            // Report permissions
            var reportPermission = analyticsReportingGroup.AddPermission(
                AnalyticsReportingPermissions.Report.Default,
                L("Permission:Report")
            );
            reportPermission.AddChild(
                AnalyticsReportingPermissions.Report.Create,
                L("Permission:Report.Create")
            );
            reportPermission.AddChild(
                AnalyticsReportingPermissions.Report.Update,
                L("Permission:Report.Update")
            );
            reportPermission.AddChild(
                AnalyticsReportingPermissions.Report.Delete,
                L("Permission:Report.Delete")
            );
            reportPermission.AddChild(
                AnalyticsReportingPermissions.Report.View,
                L("Permission:Report.View")
            );
            reportPermission.AddChild(
                AnalyticsReportingPermissions.Report.Generate,
                L("Permission:Report.Generate")
            );
            reportPermission.AddChild(
                AnalyticsReportingPermissions.Report.Schedule,
                L("Permission:Report.Schedule")
            );

            // KPI permissions
            var kpiPermission = analyticsReportingGroup.AddPermission(
                AnalyticsReportingPermissions.Kpi.Default,
                L("Permission:Kpi")
            );
            kpiPermission.AddChild(
                AnalyticsReportingPermissions.Kpi.Create,
                L("Permission:Kpi.Create")
            );
            kpiPermission.AddChild(
                AnalyticsReportingPermissions.Kpi.Update,
                L("Permission:Kpi.Update")
            );
            kpiPermission.AddChild(
                AnalyticsReportingPermissions.Kpi.Delete,
                L("Permission:Kpi.Delete")
            );
            kpiPermission.AddChild(
                AnalyticsReportingPermissions.Kpi.View,
                L("Permission:Kpi.View")
            );
            kpiPermission.AddChild(
                AnalyticsReportingPermissions.Kpi.ManageThresholds,
                L("Permission:Kpi.ManageThresholds")
            );

            // DataSource permissions
            var dataSourcePermission = analyticsReportingGroup.AddPermission(
                AnalyticsReportingPermissions.DataSource.Default,
                L("Permission:DataSource")
            );
            dataSourcePermission.AddChild(
                AnalyticsReportingPermissions.DataSource.Create,
                L("Permission:DataSource.Create")
            );
            dataSourcePermission.AddChild(
                AnalyticsReportingPermissions.DataSource.Update,
                L("Permission:DataSource.Update")
            );
            dataSourcePermission.AddChild(
                AnalyticsReportingPermissions.DataSource.Delete,
                L("Permission:DataSource.Delete")
            );
            dataSourcePermission.AddChild(
                AnalyticsReportingPermissions.DataSource.View,
                L("Permission:DataSource.View")
            );
            dataSourcePermission.AddChild(
                AnalyticsReportingPermissions.DataSource.ConfigureTransformations,
                L("Permission:DataSource.ConfigureTransformations")
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AnalyticsReportingResource>(name);
        }
    }
}
