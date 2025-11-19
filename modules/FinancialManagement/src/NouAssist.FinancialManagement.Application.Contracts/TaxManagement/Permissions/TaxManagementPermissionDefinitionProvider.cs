using NouAssist.FinancialManagement.TaxManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.FinancialManagement.TaxManagement.Permissions
{
    public class TaxManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var taxManagementGroup = context.AddGroup(
                TaxManagementPermissions.GroupName,
                L("Permission:TaxManagement"));

            var taxReturns = taxManagementGroup.AddPermission(
                TaxManagementPermissions.TaxReturns.Default,
                L("Permission:TaxReturns"));
            taxReturns.AddChild(TaxManagementPermissions.TaxReturns.Create, L("Permission:Create"));
            taxReturns.AddChild(TaxManagementPermissions.TaxReturns.Update, L("Permission:Update"));
            taxReturns.AddChild(TaxManagementPermissions.TaxReturns.Delete, L("Permission:Delete"));
            taxReturns.AddChild(TaxManagementPermissions.TaxReturns.File, L("Permission:File"));
            taxReturns.AddChild(TaxManagementPermissions.TaxReturns.ViewDocuments, L("Permission:ViewDocuments"));

            var taxProfiles = taxManagementGroup.AddPermission(
                TaxManagementPermissions.TaxProfiles.Default,
                L("Permission:TaxProfiles"));
            taxProfiles.AddChild(TaxManagementPermissions.TaxProfiles.Manage, L("Permission:Manage"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<TaxManagementResource>(name);
        }
    }
}
