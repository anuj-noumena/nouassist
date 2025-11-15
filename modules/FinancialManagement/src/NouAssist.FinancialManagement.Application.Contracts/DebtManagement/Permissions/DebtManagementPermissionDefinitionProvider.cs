using NouAssist.DebtManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.DebtManagement.Permissions
{
    public class DebtManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var debtManagementGroup = context.AddGroup(
                DebtManagementPermissions.GroupName,
                L("Permission:DebtManagement"));

            var debtAccounts = debtManagementGroup.AddPermission(
                DebtManagementPermissions.DebtAccounts.Default,
                L("Permission:DebtAccounts"));
            debtAccounts.AddChild(DebtManagementPermissions.DebtAccounts.Create, L("Permission:Create"));
            debtAccounts.AddChild(DebtManagementPermissions.DebtAccounts.Update, L("Permission:Update"));
            debtAccounts.AddChild(DebtManagementPermissions.DebtAccounts.Delete, L("Permission:Delete"));
            debtAccounts.AddChild(DebtManagementPermissions.DebtAccounts.RecordPayment, L("Permission:RecordPayment"));

            var payoffPlans = debtManagementGroup.AddPermission(
                DebtManagementPermissions.PayoffPlans.Default,
                L("Permission:PayoffPlans"));
            payoffPlans.AddChild(DebtManagementPermissions.PayoffPlans.Create, L("Permission:Create"));
            payoffPlans.AddChild(DebtManagementPermissions.PayoffPlans.Update, L("Permission:Update"));
            payoffPlans.AddChild(DebtManagementPermissions.PayoffPlans.Delete, L("Permission:Delete"));
            payoffPlans.AddChild(DebtManagementPermissions.PayoffPlans.Execute, L("Permission:Execute"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DebtManagementResource>(name);
        }
    }
}
