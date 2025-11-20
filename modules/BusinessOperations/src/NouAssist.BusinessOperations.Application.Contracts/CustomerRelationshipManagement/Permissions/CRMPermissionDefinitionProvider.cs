using NouAssist.BusinessOperations.CustomerRelationshipManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Permissions
{
    public class CRMPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            // Define a top-level group for the module
            var crmGroup = context.AddGroup(CRMPermissions.GroupName, L("Permission:CRM"));

            // Lead permissions
            var leadPermission = crmGroup.AddPermission(CRMPermissions.Lead.Default, L("Permission:Lead"));
            leadPermission.AddChild(CRMPermissions.Lead.Create, L("Permission:Lead.Create"));
            leadPermission.AddChild(CRMPermissions.Lead.Update, L("Permission:Lead.Update"));
            leadPermission.AddChild(CRMPermissions.Lead.Delete, L("Permission:Lead.Delete"));
            leadPermission.AddChild(CRMPermissions.Lead.View, L("Permission:Lead.View"));
            leadPermission.AddChild(CRMPermissions.Lead.Convert, L("Permission:Lead.Convert"));

            // Contact permissions
            var contactPermission = crmGroup.AddPermission(CRMPermissions.Contact.Default, L("Permission:Contact"));
            contactPermission.AddChild(CRMPermissions.Contact.Create, L("Permission:Contact.Create"));
            contactPermission.AddChild(CRMPermissions.Contact.Update, L("Permission:Contact.Update"));
            contactPermission.AddChild(CRMPermissions.Contact.Delete, L("Permission:Contact.Delete"));
            contactPermission.AddChild(CRMPermissions.Contact.View, L("Permission:Contact.View"));
            contactPermission.AddChild(CRMPermissions.Contact.ManageGroups, L("Permission:Contact.ManageGroups"));

            // Organization permissions
            var organizationPermission = crmGroup.AddPermission(CRMPermissions.Organization.Default, L("Permission:Organization"));
            organizationPermission.AddChild(CRMPermissions.Organization.Create, L("Permission:Organization.Create"));
            organizationPermission.AddChild(CRMPermissions.Organization.Update, L("Permission:Organization.Update"));
            organizationPermission.AddChild(CRMPermissions.Organization.Delete, L("Permission:Organization.Delete"));
            organizationPermission.AddChild(CRMPermissions.Organization.View, L("Permission:Organization.View"));

            // Interaction permissions
            var interactionPermission = crmGroup.AddPermission(CRMPermissions.Interaction.Default, L("Permission:Interaction"));
            interactionPermission.AddChild(CRMPermissions.Interaction.Create, L("Permission:Interaction.Create"));
            interactionPermission.AddChild(CRMPermissions.Interaction.Update, L("Permission:Interaction.Update"));
            interactionPermission.AddChild(CRMPermissions.Interaction.Delete, L("Permission:Interaction.Delete"));
            interactionPermission.AddChild(CRMPermissions.Interaction.View, L("Permission:Interaction.View"));
            interactionPermission.AddChild(CRMPermissions.Interaction.ManageFollowUps, L("Permission:Interaction.ManageFollowUps"));

            // Automation permissions
            var automationPermission = crmGroup.AddPermission(CRMPermissions.Automation.Default, L("Permission:Automation"));
            automationPermission.AddChild(CRMPermissions.Automation.Create, L("Permission:Automation.Create"));
            automationPermission.AddChild(CRMPermissions.Automation.Update, L("Permission:Automation.Update"));
            automationPermission.AddChild(CRMPermissions.Automation.Delete, L("Permission:Automation.Delete"));
            automationPermission.AddChild(CRMPermissions.Automation.View, L("Permission:Automation.View"));
            automationPermission.AddChild(CRMPermissions.Automation.Execute, L("Permission:Automation.Execute"));

            // Communication Template permissions
            var templatePermission = crmGroup.AddPermission(CRMPermissions.CommunicationTemplate.Default, L("Permission:CommunicationTemplate"));
            templatePermission.AddChild(CRMPermissions.CommunicationTemplate.Create, L("Permission:CommunicationTemplate.Create"));
            templatePermission.AddChild(CRMPermissions.CommunicationTemplate.Update, L("Permission:CommunicationTemplate.Update"));
            templatePermission.AddChild(CRMPermissions.CommunicationTemplate.Delete, L("Permission:CommunicationTemplate.Delete"));
            templatePermission.AddChild(CRMPermissions.CommunicationTemplate.View, L("Permission:CommunicationTemplate.View"));

            // Analytics permissions
            var analyticsPermission = crmGroup.AddPermission(CRMPermissions.Analytics.Default, L("Permission:Analytics"));
            analyticsPermission.AddChild(CRMPermissions.Analytics.ViewLeadMetrics, L("Permission:Analytics.ViewLeadMetrics"));
            analyticsPermission.AddChild(CRMPermissions.Analytics.ViewContactMetrics, L("Permission:Analytics.ViewContactMetrics"));
            analyticsPermission.AddChild(CRMPermissions.Analytics.ViewInteractionMetrics, L("Permission:Analytics.ViewInteractionMetrics"));
            analyticsPermission.AddChild(CRMPermissions.Analytics.ExportReports, L("Permission:Analytics.ExportReports"));

            // Integration permissions
            var integrationPermission = crmGroup.AddPermission(CRMPermissions.Integration.Default, L("Permission:Integration"));
            integrationPermission.AddChild(CRMPermissions.Integration.ConfigureExternalSystems, L("Permission:Integration.ConfigureExternalSystems"));
            integrationPermission.AddChild(CRMPermissions.Integration.ImportData, L("Permission:Integration.ImportData"));
            integrationPermission.AddChild(CRMPermissions.Integration.ExportData, L("Permission:Integration.ExportData"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CRMResource>(name);
        }
    }
}
