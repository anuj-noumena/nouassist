using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Permissions
{
    public class GrowthExperimentationPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            // Define a top-level group for the module
            var moduleGroup = context.AddGroup(
                GrowthExperimentationPermissions.GroupName, 
                L("Permission:GrowthExperimentation"));

            // Define permissions for GrowthExperiment
            var experimentPermissionGroup = moduleGroup.AddPermission(
                GrowthExperimentationPermissions.GrowthExperiment.Default, 
                L("Permission:GrowthExperiment"));

            experimentPermissionGroup.AddChild(
                GrowthExperimentationPermissions.GrowthExperiment.Create, 
                L("Permission:GrowthExperiment.Create"));
            experimentPermissionGroup.AddChild(
                GrowthExperimentationPermissions.GrowthExperiment.Update, 
                L("Permission:GrowthExperiment.Update"));
            experimentPermissionGroup.AddChild(
                GrowthExperimentationPermissions.GrowthExperiment.Delete, 
                L("Permission:GrowthExperiment.Delete"));
            experimentPermissionGroup.AddChild(
                GrowthExperimentationPermissions.GrowthExperiment.View, 
                L("Permission:GrowthExperiment.View"));
            experimentPermissionGroup.AddChild(
                GrowthExperimentationPermissions.GrowthExperiment.ManageResults, 
                L("Permission:GrowthExperiment.ManageResults"));
            experimentPermissionGroup.AddChild(
                GrowthExperimentationPermissions.GrowthExperiment.AnalyzeResults, 
                L("Permission:GrowthExperiment.AnalyzeResults"));

            // Define permissions for MarketingChannel
            var channelPermissionGroup = moduleGroup.AddPermission(
                GrowthExperimentationPermissions.MarketingChannel.Default, 
                L("Permission:MarketingChannel"));

            channelPermissionGroup.AddChild(
                GrowthExperimentationPermissions.MarketingChannel.Create, 
                L("Permission:MarketingChannel.Create"));
            channelPermissionGroup.AddChild(
                GrowthExperimentationPermissions.MarketingChannel.Update, 
                L("Permission:MarketingChannel.Update"));
            channelPermissionGroup.AddChild(
                GrowthExperimentationPermissions.MarketingChannel.Delete, 
                L("Permission:MarketingChannel.Delete"));
            channelPermissionGroup.AddChild(
                GrowthExperimentationPermissions.MarketingChannel.View, 
                L("Permission:MarketingChannel.View"));
            channelPermissionGroup.AddChild(
                GrowthExperimentationPermissions.MarketingChannel.ManageMetrics, 
                L("Permission:MarketingChannel.ManageMetrics"));

            // Define permissions for ConversionFunnel
            var funnelPermissionGroup = moduleGroup.AddPermission(
                GrowthExperimentationPermissions.ConversionFunnel.Default, 
                L("Permission:ConversionFunnel"));

            funnelPermissionGroup.AddChild(
                GrowthExperimentationPermissions.ConversionFunnel.Create, 
                L("Permission:ConversionFunnel.Create"));
            funnelPermissionGroup.AddChild(
                GrowthExperimentationPermissions.ConversionFunnel.Update, 
                L("Permission:ConversionFunnel.Update"));
            funnelPermissionGroup.AddChild(
                GrowthExperimentationPermissions.ConversionFunnel.Delete, 
                L("Permission:ConversionFunnel.Delete"));
            funnelPermissionGroup.AddChild(
                GrowthExperimentationPermissions.ConversionFunnel.View, 
                L("Permission:ConversionFunnel.View"));
            funnelPermissionGroup.AddChild(
                GrowthExperimentationPermissions.ConversionFunnel.ManageStages, 
                L("Permission:ConversionFunnel.ManageStages"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<GrowthExperimentationResource>(name);
        }
    }
}
