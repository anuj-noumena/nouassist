using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Permissions
{
    public class PerformanceFinancialModelingPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var performanceFinancialModelingGroup = context.AddGroup(
                PerformanceFinancialModelingPermissions.GroupName,
                L("Permission:PerformanceFinancialModeling"));

            // Tracked Metrics permissions
            var trackedMetricsPermission = performanceFinancialModelingGroup.AddPermission(
                PerformanceFinancialModelingPermissions.TrackedMetrics.Default,
                L("Permission:TrackedMetrics"));
            trackedMetricsPermission.AddChild(
                PerformanceFinancialModelingPermissions.TrackedMetrics.Create,
                L("Permission:TrackedMetrics.Create"));
            trackedMetricsPermission.AddChild(
                PerformanceFinancialModelingPermissions.TrackedMetrics.Update,
                L("Permission:TrackedMetrics.Update"));
            trackedMetricsPermission.AddChild(
                PerformanceFinancialModelingPermissions.TrackedMetrics.Delete,
                L("Permission:TrackedMetrics.Delete"));
            trackedMetricsPermission.AddChild(
                PerformanceFinancialModelingPermissions.TrackedMetrics.View,
                L("Permission:TrackedMetrics.View"));
            trackedMetricsPermission.AddChild(
                PerformanceFinancialModelingPermissions.TrackedMetrics.ManageDataPoints,
                L("Permission:TrackedMetrics.ManageDataPoints"));
            trackedMetricsPermission.AddChild(
                PerformanceFinancialModelingPermissions.TrackedMetrics.ManageGoals,
                L("Permission:TrackedMetrics.ManageGoals"));

            // Metric Definitions permissions
            var metricDefinitionsPermission = performanceFinancialModelingGroup.AddPermission(
                PerformanceFinancialModelingPermissions.MetricDefinitions.Default,
                L("Permission:MetricDefinitions"));
            metricDefinitionsPermission.AddChild(
                PerformanceFinancialModelingPermissions.MetricDefinitions.Create,
                L("Permission:MetricDefinitions.Create"));
            metricDefinitionsPermission.AddChild(
                PerformanceFinancialModelingPermissions.MetricDefinitions.Update,
                L("Permission:MetricDefinitions.Update"));
            metricDefinitionsPermission.AddChild(
                PerformanceFinancialModelingPermissions.MetricDefinitions.Delete,
                L("Permission:MetricDefinitions.Delete"));
            metricDefinitionsPermission.AddChild(
                PerformanceFinancialModelingPermissions.MetricDefinitions.View,
                L("Permission:MetricDefinitions.View"));

            // Metric Categories permissions
            var metricCategoriesPermission = performanceFinancialModelingGroup.AddPermission(
                PerformanceFinancialModelingPermissions.MetricCategories.Default,
                L("Permission:MetricCategories"));
            metricCategoriesPermission.AddChild(
                PerformanceFinancialModelingPermissions.MetricCategories.Create,
                L("Permission:MetricCategories.Create"));
            metricCategoriesPermission.AddChild(
                PerformanceFinancialModelingPermissions.MetricCategories.Update,
                L("Permission:MetricCategories.Update"));
            metricCategoriesPermission.AddChild(
                PerformanceFinancialModelingPermissions.MetricCategories.Delete,
                L("Permission:MetricCategories.Delete"));
            metricCategoriesPermission.AddChild(
                PerformanceFinancialModelingPermissions.MetricCategories.View,
                L("Permission:MetricCategories.View"));

            // Financial Models permissions
            var financialModelsPermission = performanceFinancialModelingGroup.AddPermission(
                PerformanceFinancialModelingPermissions.FinancialModels.Default,
                L("Permission:FinancialModels"));
            financialModelsPermission.AddChild(
                PerformanceFinancialModelingPermissions.FinancialModels.Create,
                L("Permission:FinancialModels.Create"));
            financialModelsPermission.AddChild(
                PerformanceFinancialModelingPermissions.FinancialModels.Update,
                L("Permission:FinancialModels.Update"));
            financialModelsPermission.AddChild(
                PerformanceFinancialModelingPermissions.FinancialModels.Delete,
                L("Permission:FinancialModels.Delete"));
            financialModelsPermission.AddChild(
                PerformanceFinancialModelingPermissions.FinancialModels.View,
                L("Permission:FinancialModels.View"));
            financialModelsPermission.AddChild(
                PerformanceFinancialModelingPermissions.FinancialModels.ManageScenarios,
                L("Permission:FinancialModels.ManageScenarios"));
            financialModelsPermission.AddChild(
                PerformanceFinancialModelingPermissions.FinancialModels.ManageAssumptions,
                L("Permission:FinancialModels.ManageAssumptions"));
            financialModelsPermission.AddChild(
                PerformanceFinancialModelingPermissions.FinancialModels.Calculate,
                L("Permission:FinancialModels.Calculate"));

            // Financial Calculations permissions
            var calculationsPermission = performanceFinancialModelingGroup.AddPermission(
                PerformanceFinancialModelingPermissions.FinancialCalculations.Default,
                L("Permission:FinancialCalculations"));
            calculationsPermission.AddChild(
                PerformanceFinancialModelingPermissions.FinancialCalculations.Execute,
                L("Permission:FinancialCalculations.Execute"));
            calculationsPermission.AddChild(
                PerformanceFinancialModelingPermissions.FinancialCalculations.ViewResults,
                L("Permission:FinancialCalculations.ViewResults"));
            calculationsPermission.AddChild(
                PerformanceFinancialModelingPermissions.FinancialCalculations.SensitivityAnalysis,
                L("Permission:FinancialCalculations.SensitivityAnalysis"));
            calculationsPermission.AddChild(
                PerformanceFinancialModelingPermissions.FinancialCalculations.RunwayProjections,
                L("Permission:FinancialCalculations.RunwayProjections"));

            // Analytics permissions
            var analyticsPermission = performanceFinancialModelingGroup.AddPermission(
                PerformanceFinancialModelingPermissions.Analytics.Default,
                L("Permission:Analytics"));
            analyticsPermission.AddChild(
                PerformanceFinancialModelingPermissions.Analytics.ViewTrends,
                L("Permission:Analytics.ViewTrends"));
            analyticsPermission.AddChild(
                PerformanceFinancialModelingPermissions.Analytics.ViewDashboard,
                L("Permission:Analytics.ViewDashboard"));
            analyticsPermission.AddChild(
                PerformanceFinancialModelingPermissions.Analytics.ExportData,
                L("Permission:Analytics.ExportData"));
            analyticsPermission.AddChild(
                PerformanceFinancialModelingPermissions.Analytics.ViewReports,
                L("Permission:Analytics.ViewReports"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PerformanceFinancialModelingResource>(name);
        }
    }
}
