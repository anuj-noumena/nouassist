using NouAssist.StartupEntrepreneurship.MarketIntelligence.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Permissions
{
    public class MarketIntelligencePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            // Define a top-level group for the module
            var moduleGroup = context.AddGroup(
                MarketIntelligencePermissions.GroupName, 
                L("Permission:MarketIntelligence"));

            // Define permissions for Competitor Management
            var competitorPermissionGroup = moduleGroup.AddPermission(
                MarketIntelligencePermissions.Competitor.Default, 
                L("Permission:Competitor"));

            competitorPermissionGroup.AddChild(
                MarketIntelligencePermissions.Competitor.Create, 
                L("Permission:Competitor.Create"));
            competitorPermissionGroup.AddChild(
                MarketIntelligencePermissions.Competitor.Update, 
                L("Permission:Competitor.Edit"));
            competitorPermissionGroup.AddChild(
                MarketIntelligencePermissions.Competitor.Delete, 
                L("Permission:Competitor.Delete"));
            competitorPermissionGroup.AddChild(
                MarketIntelligencePermissions.Competitor.View, 
                L("Permission:Competitor.View"));
            competitorPermissionGroup.AddChild(
                MarketIntelligencePermissions.Competitor.ManageStatus, 
                L("Permission:Competitor.ManageStatus"));
            competitorPermissionGroup.AddChild(
                MarketIntelligencePermissions.Competitor.ManageThreatLevel, 
                L("Permission:Competitor.ManageThreatLevel"));

            // Define permissions for Market Trend Management
            var marketTrendPermissionGroup = moduleGroup.AddPermission(
                MarketIntelligencePermissions.MarketTrend.Default, 
                L("Permission:MarketTrend"));

            marketTrendPermissionGroup.AddChild(
                MarketIntelligencePermissions.MarketTrend.Create, 
                L("Permission:MarketTrend.Create"));
            marketTrendPermissionGroup.AddChild(
                MarketIntelligencePermissions.MarketTrend.Update, 
                L("Permission:MarketTrend.Edit"));
            marketTrendPermissionGroup.AddChild(
                MarketIntelligencePermissions.MarketTrend.Delete, 
                L("Permission:MarketTrend.Delete"));
            marketTrendPermissionGroup.AddChild(
                MarketIntelligencePermissions.MarketTrend.View, 
                L("Permission:MarketTrend.View"));
            marketTrendPermissionGroup.AddChild(
                MarketIntelligencePermissions.MarketTrend.ManageDataSources, 
                L("Permission:MarketTrend.ManageDataSources"));

            // Define permissions for Customer Segment Profile Management
            var customerSegmentPermissionGroup = moduleGroup.AddPermission(
                MarketIntelligencePermissions.CustomerSegment.Default, 
                L("Permission:CustomerSegment"));

            customerSegmentPermissionGroup.AddChild(
                MarketIntelligencePermissions.CustomerSegment.Create, 
                L("Permission:CustomerSegment.Create"));
            customerSegmentPermissionGroup.AddChild(
                MarketIntelligencePermissions.CustomerSegment.Update, 
                L("Permission:CustomerSegment.Edit"));
            customerSegmentPermissionGroup.AddChild(
                MarketIntelligencePermissions.CustomerSegment.Delete, 
                L("Permission:CustomerSegment.Delete"));
            customerSegmentPermissionGroup.AddChild(
                MarketIntelligencePermissions.CustomerSegment.View, 
                L("Permission:CustomerSegment.View"));
            customerSegmentPermissionGroup.AddChild(
                MarketIntelligencePermissions.CustomerSegment.ManageCharacteristics, 
                L("Permission:CustomerSegment.ManageCharacteristics"));

            // Define permissions for SWOT Analysis Management
            var swotAnalysisPermissionGroup = moduleGroup.AddPermission(
                MarketIntelligencePermissions.SwotAnalysis.Default, 
                L("Permission:SwotAnalysis"));

            swotAnalysisPermissionGroup.AddChild(
                MarketIntelligencePermissions.SwotAnalysis.Create, 
                L("Permission:SwotAnalysis.Create"));
            swotAnalysisPermissionGroup.AddChild(
                MarketIntelligencePermissions.SwotAnalysis.Update, 
                L("Permission:SwotAnalysis.Edit"));
            swotAnalysisPermissionGroup.AddChild(
                MarketIntelligencePermissions.SwotAnalysis.Delete, 
                L("Permission:SwotAnalysis.Delete"));
            swotAnalysisPermissionGroup.AddChild(
                MarketIntelligencePermissions.SwotAnalysis.View, 
                L("Permission:SwotAnalysis.View"));
            swotAnalysisPermissionGroup.AddChild(
                MarketIntelligencePermissions.SwotAnalysis.ManageItems, 
                L("Permission:SwotAnalysis.ManageItems"));
            swotAnalysisPermissionGroup.AddChild(
                MarketIntelligencePermissions.SwotAnalysis.Complete, 
                L("Permission:SwotAnalysis.Complete"));

            // Define permissions for Market Intelligence Dashboard
            var dashboardPermissionGroup = moduleGroup.AddPermission(
                MarketIntelligencePermissions.Dashboard.Default, 
                L("Permission:Dashboard"));

            dashboardPermissionGroup.AddChild(
                MarketIntelligencePermissions.Dashboard.View, 
                L("Permission:Dashboard.View"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MarketIntelligenceResource>(name);
        }
    }
}
