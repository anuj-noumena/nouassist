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
            var generalLedger = financialManagementGroup.AddPermission(
                FinancialManagementPermissions.GeneralLedger.Default,
                L("Permission:GeneralLedger"));

            generalLedger.AddChild(
                FinancialManagementPermissions.GeneralLedger.ViewAccounts,
                L("Permission:ViewAccounts"));

            generalLedger.AddChild(
                FinancialManagementPermissions.GeneralLedger.ManageAccounts,
                L("Permission:ManageAccounts"));

            generalLedger.AddChild(
                FinancialManagementPermissions.GeneralLedger.PostJournalEntries,
                L("Permission:PostJournalEntries"));

            generalLedger.AddChild(
                FinancialManagementPermissions.GeneralLedger.ReverseJournalEntries,
                L("Permission:ReverseJournalEntries"));

            generalLedger.AddChild(
                FinancialManagementPermissions.GeneralLedger.ClosePeriods,
                L("Permission:ClosePeriods"));

            // Accounts Payable Permissions
            var accountsPayable = financialManagementGroup.AddPermission(
                FinancialManagementPermissions.AccountsPayable.Default,
                L("Permission:AccountsPayable"));

            accountsPayable.AddChild(
                FinancialManagementPermissions.AccountsPayable.ViewBills,
                L("Permission:ViewBills"));

            accountsPayable.AddChild(
                FinancialManagementPermissions.AccountsPayable.CreateBills,
                L("Permission:CreateBills"));

            accountsPayable.AddChild(
                FinancialManagementPermissions.AccountsPayable.ApproveBills,
                L("Permission:ApproveBills"));

            accountsPayable.AddChild(
                FinancialManagementPermissions.AccountsPayable.ProcessPayments,
                L("Permission:ProcessPayments"));

            // Accounts Receivable Permissions
            var accountsReceivable = financialManagementGroup.AddPermission(
                FinancialManagementPermissions.AccountsReceivable.Default,
                L("Permission:AccountsReceivable"));

            accountsReceivable.AddChild(
                FinancialManagementPermissions.AccountsReceivable.ViewInvoices,
                L("Permission:ViewInvoices"));

            accountsReceivable.AddChild(
                FinancialManagementPermissions.AccountsReceivable.CreateInvoices,
                L("Permission:CreateInvoices"));

            accountsReceivable.AddChild(
                FinancialManagementPermissions.AccountsReceivable.SendInvoices,
                L("Permission:SendInvoices"));

            accountsReceivable.AddChild(
                FinancialManagementPermissions.AccountsReceivable.ProcessPayments,
                L("Permission:ProcessPayments"));

            // Financial Planning Permissions
            var financialPlanning = financialManagementGroup.AddPermission(
                FinancialManagementPermissions.FinancialPlanning.Default,
                L("Permission:FinancialPlanning"));

            financialPlanning.AddChild(
                FinancialManagementPermissions.FinancialPlanning.ViewExpenses,
                L("Permission:ViewExpenses"));

            financialPlanning.AddChild(
                FinancialManagementPermissions.FinancialPlanning.CreateExpenses,
                L("Permission:CreateExpenses"));

            financialPlanning.AddChild(
                FinancialManagementPermissions.FinancialPlanning.ApproveExpenses,
                L("Permission:ApproveExpenses"));

            financialPlanning.AddChild(
                FinancialManagementPermissions.FinancialPlanning.ManageBudgets,
                L("Permission:ManageBudgets"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FinancialManagementResource>(name);
        }
    }
}
