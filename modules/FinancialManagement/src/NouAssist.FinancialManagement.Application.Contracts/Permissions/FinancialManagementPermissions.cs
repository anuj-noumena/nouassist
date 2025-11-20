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
            public const string CreateJournalEntries = Default + ".CreateJournalEntries";
            public const string PostJournalEntries = Default + ".PostJournalEntries";
            public const string ReverseJournalEntries = Default + ".ReverseJournalEntries";
            public const string ManagePeriods = Default + ".ManagePeriods";
            public const string ClosePeriods = Default + ".ClosePeriods";
            public const string ViewFinancialReports = Default + ".ViewFinancialReports";
            public const string ReconcileBankStatements = Default + ".ReconcileBankStatements";
            public const string ViewStatements = Default + ".ViewStatements";
            public const string CreateStatements = Default + ".CreateStatements";
            public const string ImportStatements = Default + ".ImportStatements";
            public const string ReconcileStatements = Default + ".ReconcileStatements";
        }

        public static class AccountsPayable
        {
            public const string Default = GroupName + ".AccountsPayable";
            public const string ViewBills = Default + ".ViewBills";
            public const string CreateBills = Default + ".CreateBills";
            public const string ApproveBills = Default + ".ApproveBills";
            public const string PayBills = Default + ".PayBills";
            public const string ManageVendors = Default + ".ManageVendors";
            public const string ViewReports = Default + ".ViewReports";
        }

        public static class AccountsReceivable
        {
            public const string Default = GroupName + ".AccountsReceivable";
            public const string ViewInvoices = Default + ".ViewInvoices";
            public const string CreateInvoices = Default + ".CreateInvoices";
            public const string SendInvoices = Default + ".SendInvoices";
            public const string RecordPayments = Default + ".RecordPayments";
            public const string ManageCustomers = Default + ".ManageCustomers";
            public const string ManageCollections = Default + ".ManageCollections";
            public const string ViewReports = Default + ".ViewReports";
            public const string ViewPayments = Default + ".ViewPayments";
            public const string CreatePayments = Default + ".CreatePayments";
        }

        public static class FinancialPlanning
        {
            public const string Default = GroupName + ".FinancialPlanning";
            public const string ViewExpenses = Default + ".ViewExpenses";
            public const string CreateExpenses = Default + ".CreateExpenses";
            public const string ApproveExpenses = Default + ".ApproveExpenses";
            public const string ManageBudgets = Default + ".ManageBudgets";
            public const string ManageGoals = Default + ".ManageGoals";
            public const string LinkBankAccounts = Default + ".LinkBankAccounts";
            public const string ViewReports = Default + ".ViewReports";
            public const string ViewAnalytics = Default + ".ViewAnalytics";
            public const string CreateReports = Default + ".CreateReports";
            public const string SubmitReports = Default + ".SubmitReports";
            public const string ApproveReports = Default + ".ApproveReports";
            public const string ViewGoals = Default + ".ViewGoals";
            public const string CreateGoals = Default + ".CreateGoals";
            public const string UpdateGoals = Default + ".UpdateGoals";
            public const string CloseGoals = Default + ".CloseGoals";
            public const string ViewAccounts = Default + ".ViewAccounts";
            public const string CreateAccounts = Default + ".CreateAccounts";
            public const string UpdateAccounts = Default + ".UpdateAccounts";
        }
    }
}
