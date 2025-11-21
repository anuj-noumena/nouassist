using NouAssist.IntelligenceStrategy.AnalyticsReporting.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Analytics.Permissions
{
    public class AnalyticsPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var analyticsGroup = context.AddGroup(AnalyticsPermissions.GroupName, L("Permission:Analytics"));
            
            var dashboardsPermission = analyticsGroup.AddPermission(AnalyticsPermissions.Dashboards.Default, L("Permission:Analytics.Dashboards"));
            dashboardsPermission.AddChild(AnalyticsPermissions.Dashboards.Create, L("Permission:Analytics.Dashboards.Create"));
            dashboardsPermission.AddChild(AnalyticsPermissions.Dashboards.Edit, L("Permission:Analytics.Dashboards.Edit"));
            dashboardsPermission.AddChild(AnalyticsPermissions.Dashboards.Delete, L("Permission:Analytics.Dashboards.Delete"));
            
            var reportsPermission = analyticsGroup.AddPermission(AnalyticsPermissions.Reports.Default, L("Permission:Analytics.Reports"));
            reportsPermission.AddChild(AnalyticsPermissions.Reports.Create, L("Permission:Analytics.Reports.Create"));
            reportsPermission.AddChild(AnalyticsPermissions.Reports.Edit, L("Permission:Analytics.Reports.Edit"));
            reportsPermission.AddChild(AnalyticsPermissions.Reports.Delete, L("Permission:Analytics.Reports.Delete"));
            reportsPermission.AddChild(AnalyticsPermissions.Reports.Generate, L("Permission:Analytics.Reports.Generate"));
            
            var kpisPermission = analyticsGroup.AddPermission(AnalyticsPermissions.KPIs.Default, L("Permission:Analytics.KPIs"));
            kpisPermission.AddChild(AnalyticsPermissions.KPIs.Create, L("Permission:Analytics.KPIs.Create"));
            kpisPermission.AddChild(AnalyticsPermissions.KPIs.Edit, L("Permission:Analytics.KPIs.Edit"));
            kpisPermission.AddChild(AnalyticsPermissions.KPIs.Delete, L("Permission:Analytics.KPIs.Delete"));
            
            var dataSourcesPermission = analyticsGroup.AddPermission(AnalyticsPermissions.DataSources.Default, L("Permission:Analytics.DataSources"));
            dataSourcesPermission.AddChild(AnalyticsPermissions.DataSources.Create, L("Permission:Analytics.DataSources.Create"));
            dataSourcesPermission.AddChild(AnalyticsPermissions.DataSources.Edit, L("Permission:Analytics.DataSources.Edit"));
            dataSourcesPermission.AddChild(AnalyticsPermissions.DataSources.Delete, L("Permission:Analytics.DataSources.Delete"));
        }
        
        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AnalyticsResource>(name);
        }
    }
}
