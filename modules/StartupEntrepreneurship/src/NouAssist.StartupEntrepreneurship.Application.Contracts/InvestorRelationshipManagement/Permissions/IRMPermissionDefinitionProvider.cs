using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Permissions
{
    /// <summary>
    /// Permission definition provider for IRM module
    /// </summary>
    public class IRMPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var irmGroup = context.AddGroup(IRMPermissions.GroupName, L("Permission:IRM"));

            // Investor Management Permissions
            var investorsPermission = irmGroup.AddPermission(IRMPermissions.Investors.Default, L("Permission:Investors"));
            investorsPermission.AddChild(IRMPermissions.Investors.Create, L("Permission:Investors.Create"));
            investorsPermission.AddChild(IRMPermissions.Investors.Update, L("Permission:Investors.Update"));
            investorsPermission.AddChild(IRMPermissions.Investors.Delete, L("Permission:Investors.Delete"));
            investorsPermission.AddChild(IRMPermissions.Investors.View, L("Permission:Investors.View"));
            investorsPermission.AddChild(IRMPermissions.Investors.ManageRelationships, L("Permission:Investors.ManageRelationships"));
            investorsPermission.AddChild(IRMPermissions.Investors.ViewAnalytics, L("Permission:Investors.ViewAnalytics"));

            // Update Management Permissions
            var updatesPermission = irmGroup.AddPermission(IRMPermissions.Updates.Default, L("Permission:Updates"));
            updatesPermission.AddChild(IRMPermissions.Updates.Create, L("Permission:Updates.Create"));
            updatesPermission.AddChild(IRMPermissions.Updates.Update, L("Permission:Updates.Update"));
            updatesPermission.AddChild(IRMPermissions.Updates.Delete, L("Permission:Updates.Delete"));
            updatesPermission.AddChild(IRMPermissions.Updates.View, L("Permission:Updates.View"));
            updatesPermission.AddChild(IRMPermissions.Updates.Send, L("Permission:Updates.Send"));
            updatesPermission.AddChild(IRMPermissions.Updates.Schedule, L("Permission:Updates.Schedule"));
            updatesPermission.AddChild(IRMPermissions.Updates.ViewEngagement, L("Permission:Updates.ViewEngagement"));

            // Data Room Management Permissions
            var dataRoomsPermission = irmGroup.AddPermission(IRMPermissions.DataRooms.Default, L("Permission:DataRooms"));
            dataRoomsPermission.AddChild(IRMPermissions.DataRooms.Create, L("Permission:DataRooms.Create"));
            dataRoomsPermission.AddChild(IRMPermissions.DataRooms.Update, L("Permission:DataRooms.Update"));
            dataRoomsPermission.AddChild(IRMPermissions.DataRooms.Delete, L("Permission:DataRooms.Delete"));
            dataRoomsPermission.AddChild(IRMPermissions.DataRooms.View, L("Permission:DataRooms.View"));
            dataRoomsPermission.AddChild(IRMPermissions.DataRooms.ManageAccess, L("Permission:DataRooms.ManageAccess"));
            dataRoomsPermission.AddChild(IRMPermissions.DataRooms.ManageDocuments, L("Permission:DataRooms.ManageDocuments"));
            dataRoomsPermission.AddChild(IRMPermissions.DataRooms.ViewAnalytics, L("Permission:DataRooms.ViewAnalytics"));

            // Group Management Permissions
            var groupsPermission = irmGroup.AddPermission(IRMPermissions.Groups.Default, L("Permission:Groups"));
            groupsPermission.AddChild(IRMPermissions.Groups.Create, L("Permission:Groups.Create"));
            groupsPermission.AddChild(IRMPermissions.Groups.Update, L("Permission:Groups.Update"));
            groupsPermission.AddChild(IRMPermissions.Groups.Delete, L("Permission:Groups.Delete"));
            groupsPermission.AddChild(IRMPermissions.Groups.View, L("Permission:Groups.View"));
            groupsPermission.AddChild(IRMPermissions.Groups.ManageMembers, L("Permission:Groups.ManageMembers"));

            // Network Intelligence Permissions
            var networkPermission = irmGroup.AddPermission(IRMPermissions.NetworkIntelligence.Default, L("Permission:NetworkIntelligence"));
            networkPermission.AddChild(IRMPermissions.NetworkIntelligence.View, L("Permission:NetworkIntelligence.View"));
            networkPermission.AddChild(IRMPermissions.NetworkIntelligence.GenerateReports, L("Permission:NetworkIntelligence.GenerateReports"));
            networkPermission.AddChild(IRMPermissions.NetworkIntelligence.ViewInfluenceScores, L("Permission:NetworkIntelligence.ViewInfluenceScores"));

            // Analytics Permissions
            var analyticsPermission = irmGroup.AddPermission(IRMPermissions.Analytics.Default, L("Permission:Analytics"));
            analyticsPermission.AddChild(IRMPermissions.Analytics.ViewEngagement, L("Permission:Analytics.ViewEngagement"));
            analyticsPermission.AddChild(IRMPermissions.Analytics.ViewPerformance, L("Permission:Analytics.ViewPerformance"));
            analyticsPermission.AddChild(IRMPermissions.Analytics.ViewNetworkInsights, L("Permission:Analytics.ViewNetworkInsights"));
            analyticsPermission.AddChild(IRMPermissions.Analytics.ExportReports, L("Permission:Analytics.ExportReports"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<IRMResource>(name);
        }
    }
}
