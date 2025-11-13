namespace NouAssist.FinancialManagement.Permissions
{
    public static class FinancialManagementPermissions
    {
        public const string GroupName = "FinancialManagement";

        public static class GeneralLedger
        {
            public const string Default = GroupName + ".GeneralLedger";
            public const string ViewAccounts = Default + ".ViewAccounts";
            public const string ManageAccounts = Default + ".ManageAccounts";
            public const string PostJournalEntries = Default + ".PostJournalEntries";
            public const string ReverseJournalEntries = Default + ".ReverseJournalEntries";
            public const string ClosePeriods = Default + ".ClosePeriods";
            public const string ViewFinancialReports = Default + ".ViewFinancialReports";
        }

        public static class AccountsPayable
        {
            public const string Default = GroupName + ".AccountsPayable";
            public const string ViewBills = Default + ".ViewBills";
            public const string CreateBills = Default + ".CreateBills";
            public const string ApproveBills = Default + ".ApproveBills";
            public const string ProcessPayments = Default + ".ProcessPayments";
            public const string ManageVendors = Default + ".ManageVendors";
        }

        public static class AccountsReceivable
        {
            public const string Default = GroupName + ".AccountsReceivable";
            public const string ViewInvoices = Default + ".ViewInvoices";
            public const string CreateInvoices = Default + ".CreateInvoices";
            public const string SendInvoices = Default + ".SendInvoices";
            public const string ProcessPayments = Default + ".ProcessPayments";
            public const string ManageCustomers = Default + ".ManageCustomers";
            public const string ManageCollections = Default + ".ManageCollections";
        }

        public static class FinancialPlanning
        {
            public const string Default = GroupName + ".FinancialPlanning";
            public const string ViewExpenses = Default + ".ViewExpenses";
            public const string CreateExpenses = Default + ".CreateExpenses";
            public const string ApproveExpenses = Default + ".ApproveExpenses";
            public const string ManageBudgets = Default + ".ManageBudgets";
            public const string ManageGoals = Default + ".ManageGoals";
        }
    }
}
