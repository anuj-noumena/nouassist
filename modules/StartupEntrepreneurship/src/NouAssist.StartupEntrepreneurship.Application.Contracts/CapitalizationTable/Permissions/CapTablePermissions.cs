namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Permissions
{
    /// <summary>
    /// Permissions for the CapTable module
    /// </summary>
    public static class CapTablePermissions
    {
        public const string GroupName = "CapTable"; // Module name for grouping UI

        // Permissions for CapTable Management
        public static class CapTables
        {
            public const string Default = GroupName + ".CapTables"; // Group permission
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageValuation = Default + ".ManageValuation";
        }

        // Permissions for SecurityHolder Management
        public static class SecurityHolders
        {
            public const string Default = GroupName + ".SecurityHolders";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageVesting = Default + ".ManageVesting";
        }

        // Permissions for SecurityIssuance Management
        public static class SecurityIssuances
        {
            public const string Default = GroupName + ".SecurityIssuances";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string IssueSecurities = Default + ".IssueSecurities";
        }

        // Permissions for FundingRound Management
        public static class FundingRounds
        {
            public const string Default = GroupName + ".FundingRounds";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Close = Default + ".Close";
        }

        // Permissions for OptionPool Management
        public static class OptionPools
        {
            public const string Default = GroupName + ".OptionPools";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string GrantOptions = Default + ".GrantOptions";
        }

        // Permissions for Reports
        public static class Reports
        {
            public const string Default = GroupName + ".Reports";
            public const string ViewOwnership = Default + ".ViewOwnership";
            public const string ViewValuation = Default + ".ViewValuation";
            public const string ViewDilution = Default + ".ViewDilution";
            public const string ViewScenarios = Default + ".ViewScenarios";
        }
    }
}
