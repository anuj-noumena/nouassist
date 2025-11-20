using NouAssist.StartupEntrepreneurship.LegalCompliance.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Permissions
{
    public class LegalCompliancePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var legalGroup = context.AddGroup(
                LegalCompliancePermissions.GroupName,
                L("Permission:LegalCompliance"));

            // IP Asset permissions
            var ipPermission = legalGroup.AddPermission(
                LegalCompliancePermissions.IPAsset.Default,
                L("Permission:IPAssetManagement"));
            ipPermission.AddChild(
                LegalCompliancePermissions.IPAsset.Create,
                L("Permission:IPAsset.Create"));
            ipPermission.AddChild(
                LegalCompliancePermissions.IPAsset.Update,
                L("Permission:IPAsset.Update"));
            ipPermission.AddChild(
                LegalCompliancePermissions.IPAsset.Delete,
                L("Permission:IPAsset.Delete"));
            ipPermission.AddChild(
                LegalCompliancePermissions.IPAsset.View,
                L("Permission:IPAsset.View"));
            ipPermission.AddChild(
                LegalCompliancePermissions.IPAsset.ManageDeadlines,
                L("Permission:IPAsset.ManageDeadlines"));
            ipPermission.AddChild(
                LegalCompliancePermissions.IPAsset.ManageValuation,
                L("Permission:IPAsset.ManageValuation"));

            // Legal Agreement permissions
            var agreementPermission = legalGroup.AddPermission(
                LegalCompliancePermissions.LegalAgreement.Default,
                L("Permission:LegalAgreementManagement"));
            agreementPermission.AddChild(
                LegalCompliancePermissions.LegalAgreement.Create,
                L("Permission:LegalAgreement.Create"));
            agreementPermission.AddChild(
                LegalCompliancePermissions.LegalAgreement.Update,
                L("Permission:LegalAgreement.Update"));
            agreementPermission.AddChild(
                LegalCompliancePermissions.LegalAgreement.Delete,
                L("Permission:LegalAgreement.Delete"));
            agreementPermission.AddChild(
                LegalCompliancePermissions.LegalAgreement.View,
                L("Permission:LegalAgreement.View"));
            agreementPermission.AddChild(
                LegalCompliancePermissions.LegalAgreement.ManageTerms,
                L("Permission:LegalAgreement.ManageTerms"));
            agreementPermission.AddChild(
                LegalCompliancePermissions.LegalAgreement.ManageReminders,
                L("Permission:LegalAgreement.ManageReminders"));
            agreementPermission.AddChild(
                LegalCompliancePermissions.LegalAgreement.Renew,
                L("Permission:LegalAgreement.Renew"));
            agreementPermission.AddChild(
                LegalCompliancePermissions.LegalAgreement.Terminate,
                L("Permission:LegalAgreement.Terminate"));

            // Compliance permissions
            var compliancePermission = legalGroup.AddPermission(
                LegalCompliancePermissions.Compliance.Default,
                L("Permission:ComplianceManagement"));
            compliancePermission.AddChild(
                LegalCompliancePermissions.Compliance.Create,
                L("Permission:Compliance.Create"));
            compliancePermission.AddChild(
                LegalCompliancePermissions.Compliance.Update,
                L("Permission:Compliance.Update"));
            compliancePermission.AddChild(
                LegalCompliancePermissions.Compliance.Delete,
                L("Permission:Compliance.Delete"));
            compliancePermission.AddChild(
                LegalCompliancePermissions.Compliance.View,
                L("Permission:Compliance.View"));
            compliancePermission.AddChild(
                LegalCompliancePermissions.Compliance.UpdateStatus,
                L("Permission:Compliance.UpdateStatus"));

            // Corporate Record permissions
            var recordPermission = legalGroup.AddPermission(
                LegalCompliancePermissions.CorporateRecord.Default,
                L("Permission:CorporateRecordManagement"));
            recordPermission.AddChild(
                LegalCompliancePermissions.CorporateRecord.Create,
                L("Permission:CorporateRecord.Create"));
            recordPermission.AddChild(
                LegalCompliancePermissions.CorporateRecord.Update,
                L("Permission:CorporateRecord.Update"));
            recordPermission.AddChild(
                LegalCompliancePermissions.CorporateRecord.Delete,
                L("Permission:CorporateRecord.Delete"));
            recordPermission.AddChild(
                LegalCompliancePermissions.CorporateRecord.View,
                L("Permission:CorporateRecord.View"));
            recordPermission.AddChild(
                LegalCompliancePermissions.CorporateRecord.UpdateStatus,
                L("Permission:CorporateRecord.UpdateStatus"));

            // Document Generation permissions
            var documentPermission = legalGroup.AddPermission(
                LegalCompliancePermissions.DocumentGeneration.Default,
                L("Permission:DocumentGeneration"));
            documentPermission.AddChild(
                LegalCompliancePermissions.DocumentGeneration.ManageTemplates,
                L("Permission:DocumentGeneration.ManageTemplates"));
            documentPermission.AddChild(
                LegalCompliancePermissions.DocumentGeneration.GenerateDocuments,
                L("Permission:DocumentGeneration.GenerateDocuments"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<LegalComplianceResource>(name);
        }
    }
}
