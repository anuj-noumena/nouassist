namespace NouAssist.FinancialManagement.DebtManagement.Permissions
{
    public static class DebtManagementPermissions
    {
        public const string GroupName = "DebtManagement";

        public static class DebtAccounts
        {
            public const string Default = GroupName + ".DebtAccounts";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string RecordPayment = Default + ".RecordPayment";
        }

        public static class PayoffPlans
        {
            public const string Default = GroupName + ".PayoffPlans";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Execute = Default + ".Execute";
        }
    }
}
