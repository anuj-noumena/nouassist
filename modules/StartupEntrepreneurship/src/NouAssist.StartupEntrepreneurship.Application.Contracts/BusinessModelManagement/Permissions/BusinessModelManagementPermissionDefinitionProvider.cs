using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Permissions
{
    public class BusinessModelManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var businessModelManagementGroup = context.AddGroup(BusinessModelManagementPermissions.GroupName, L("Permission:BusinessModelManagement"));

            // Business Model Canvas Permissions
            var canvasPermission = businessModelManagementGroup.AddPermission(BusinessModelManagementPermissions.BusinessModelCanvas.Default, L("Permission:BusinessModelCanvas"));
            canvasPermission.AddChild(BusinessModelManagementPermissions.BusinessModelCanvas.Create, L("Permission:BusinessModelCanvas.Create"));
            canvasPermission.AddChild(BusinessModelManagementPermissions.BusinessModelCanvas.Update, L("Permission:BusinessModelCanvas.Update"));
            canvasPermission.AddChild(BusinessModelManagementPermissions.BusinessModelCanvas.Delete, L("Permission:BusinessModelCanvas.Delete"));
            canvasPermission.AddChild(BusinessModelManagementPermissions.BusinessModelCanvas.View, L("Permission:BusinessModelCanvas.View"));
            canvasPermission.AddChild(BusinessModelManagementPermissions.BusinessModelCanvas.CreateVersion, L("Permission:BusinessModelCanvas.CreateVersion"));
            canvasPermission.AddChild(BusinessModelManagementPermissions.BusinessModelCanvas.ManageComponents, L("Permission:BusinessModelCanvas.ManageComponents"));
            canvasPermission.AddChild(BusinessModelManagementPermissions.BusinessModelCanvas.ManageHypotheses, L("Permission:BusinessModelCanvas.ManageHypotheses"));

            // Revenue Model Permissions
            var revenueModelPermission = businessModelManagementGroup.AddPermission(BusinessModelManagementPermissions.RevenueModel.Default, L("Permission:RevenueModel"));
            revenueModelPermission.AddChild(BusinessModelManagementPermissions.RevenueModel.Create, L("Permission:RevenueModel.Create"));
            revenueModelPermission.AddChild(BusinessModelManagementPermissions.RevenueModel.Update, L("Permission:RevenueModel.Update"));
            revenueModelPermission.AddChild(BusinessModelManagementPermissions.RevenueModel.Delete, L("Permission:RevenueModel.Delete"));
            revenueModelPermission.AddChild(BusinessModelManagementPermissions.RevenueModel.View, L("Permission:RevenueModel.View"));
            revenueModelPermission.AddChild(BusinessModelManagementPermissions.RevenueModel.ManagePricing, L("Permission:RevenueModel.ManagePricing"));
            revenueModelPermission.AddChild(BusinessModelManagementPermissions.RevenueModel.ManageScenarios, L("Permission:RevenueModel.ManageScenarios"));
            revenueModelPermission.AddChild(BusinessModelManagementPermissions.RevenueModel.SimulateRevenue, L("Permission:RevenueModel.SimulateRevenue"));
            revenueModelPermission.AddChild(BusinessModelManagementPermissions.RevenueModel.ManageGtmStrategies, L("Permission:RevenueModel.ManageGtmStrategies"));

            // Strategic Pivot Permissions
            var pivotPermission = businessModelManagementGroup.AddPermission(BusinessModelManagementPermissions.StrategicPivot.Default, L("Permission:StrategicPivot"));
            pivotPermission.AddChild(BusinessModelManagementPermissions.StrategicPivot.Create, L("Permission:StrategicPivot.Create"));
            pivotPermission.AddChild(BusinessModelManagementPermissions.StrategicPivot.Update, L("Permission:StrategicPivot.Update"));
            pivotPermission.AddChild(BusinessModelManagementPermissions.StrategicPivot.Delete, L("Permission:StrategicPivot.Delete"));
            pivotPermission.AddChild(BusinessModelManagementPermissions.StrategicPivot.View, L("Permission:StrategicPivot.View"));
            pivotPermission.AddChild(BusinessModelManagementPermissions.StrategicPivot.ManageLifecycle, L("Permission:StrategicPivot.ManageLifecycle"));
            pivotPermission.AddChild(BusinessModelManagementPermissions.StrategicPivot.ManageImpactMetrics, L("Permission:StrategicPivot.ManageImpactMetrics"));
            pivotPermission.AddChild(BusinessModelManagementPermissions.StrategicPivot.ViewAnalytics, L("Permission:StrategicPivot.ViewAnalytics"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BusinessModelManagementResource>(name);
        }
    }
}
