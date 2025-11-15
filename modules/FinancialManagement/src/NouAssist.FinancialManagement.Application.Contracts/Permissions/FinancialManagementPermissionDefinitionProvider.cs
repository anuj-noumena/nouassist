using NouAssist.FinancialManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.FinancialManagement.Permissions
{
    public class FinancialManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var financialManagementGroup = context.AddGroup(
                FinancialManagementPermissions.GroupName,
                L("Permission:FinancialManagement"));

            // General Ledger Permissions
            var glPermission = financialManagementGroup.AddPermission(
                FinancialManagementPermissions.GeneralLedger.Default,
                L("Permission:GeneralLedger"));

            glPermission.AddChild(
                FinancialManagementPermissions.GeneralLedger.ViewAccounts,
                L("Permission:ViewAccounts"));
            glPermission.AddChild(
                FinancialManagementPermissions.GeneralLedger.ManageAccounts,
                L("Permission:ManageAccounts"));
            glPermission.AddChild(
                FinancialManagementPermissions.GeneralLedger.CreateJournalEntries,
                L("Permission:CreateJournalEntries"));
            glPermission.AddChild(
                FinancialManagementPermissions.GeneralLedger.PostJournalEntries,
                L("Permission:PostJournalEntries"));
            glPermission.AddChild(
                FinancialManagementPermissions.GeneralLedger.ReverseJournalEntries,
                L("Permission:ReverseJournalEntries"));
            glPermission.AddChild(
                FinancialManagementPermissions.GeneralLedger.ClosePeriods,
                L("Permission:ClosePeriods"));

            // Accounts Payable Permissions
            var apPermission = financialManagementGroup.AddPermission(
                FinancialManagementPermissions.AccountsPayable.Default,
                L("Permission:AccountsPayable"));

            apPermission.AddChild(
                FinancialManagementPermissions.AccountsPayable.ViewBills,
                L("Permission:ViewBills"));
            apPermission.AddChild(
                FinancialManagementPermissions.AccountsPayable.CreateBills,
                L("Permission:CreateBills"));
            apPermission.AddChild(
                FinancialManagementPermissions.AccountsPayable.ApproveBills,
                L("Permission:ApproveBills"));
            apPermission.AddChild(
                FinancialManagementPermissions.AccountsPayable.PayBills,
                L("Permission:PayBills"));

            // Accounts Receivable Permissions
            var arPermission = financialManagementGroup.AddPermission(
                FinancialManagementPermissions.AccountsReceivable.Default,
                L("Permission:AccountsReceivable"));

            arPermission.AddChild(
                FinancialManagementPermissions.AccountsReceivable.ViewInvoices,
                L("Permission:ViewInvoices"));
            arPermission.AddChild(
                FinancialManagementPermissions.AccountsReceivable.CreateInvoices,
                L("Permission:CreateInvoices"));
            arPermission.AddChild(
                FinancialManagementPermissions.AccountsReceivable.RecordPayments,
                L("Permission:RecordPayments"));

            // Financial Planning Permissions
            var fpPermission = financialManagementGroup.AddPermission(
                FinancialManagementPermissions.FinancialPlanning.Default,
                L("Permission:FinancialPlanning"));

            fpPermission.AddChild(
                FinancialManagementPermissions.FinancialPlanning.CreateExpenses,
                L("Permission:CreateExpenses"));
            fpPermission.AddChild(
                FinancialManagementPermissions.FinancialPlanning.ApproveExpenses,
                L("Permission:ApproveExpenses"));
            fpPermission.AddChild(
                FinancialManagementPermissions.FinancialPlanning.ManageBudgets,
                L("Permission:ManageBudgets"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FinancialManagementResource>(name);
        }
    }
}
