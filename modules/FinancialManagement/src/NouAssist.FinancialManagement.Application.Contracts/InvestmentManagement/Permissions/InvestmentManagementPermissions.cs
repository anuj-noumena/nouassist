namespace NouAssist.InvestmentManagement.Permissions
{
    public static class InvestmentManagementPermissions
    {
        public const string GroupName = "InvestmentManagement";

        public static class Portfolios
        {
            public const string Default = GroupName + ".Portfolios";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Trade = Default + ".Trade";
            public const string Rebalance = Default + ".Rebalance";
        }

        public static class InvestmentGoals
        {
            public const string Default = GroupName + ".InvestmentGoals";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
    }
}
