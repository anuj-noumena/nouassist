namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Permissions
{
    /// <summary>
    /// Permission constants for IRM module
    /// </summary>
    public static class IRMPermissions
    {
        public const string GroupName = "IRM";

        // Investor Management Permissions
        public static class Investors
        {
            public const string Default = GroupName + ".Investors";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageRelationships = Default + ".ManageRelationships";
            public const string ViewAnalytics = Default + ".ViewAnalytics";
        }

        // Update Management Permissions
        public static class Updates
        {
            public const string Default = GroupName + ".Updates";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Send = Default + ".Send";
            public const string Schedule = Default + ".Schedule";
            public const string ViewEngagement = Default + ".ViewEngagement";
        }

        // Data Room Management Permissions
        public static class DataRooms
        {
            public const string Default = GroupName + ".DataRooms";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageAccess = Default + ".ManageAccess";
            public const string ManageDocuments = Default + ".ManageDocuments";
            public const string ViewAnalytics = Default + ".ViewAnalytics";
        }

        // Group Management Permissions
        public static class Groups
        {
            public const string Default = GroupName + ".Groups";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageMembers = Default + ".ManageMembers";
        }

        // Network Intelligence Permissions
        public static class NetworkIntelligence
        {
            public const string Default = GroupName + ".NetworkIntelligence";
            public const string View = Default + ".View";
            public const string GenerateReports = Default + ".GenerateReports";
            public const string ViewInfluenceScores = Default + ".ViewInfluenceScores";
        }

        // Analytics Permissions
        public static class Analytics
        {
            public const string Default = GroupName + ".Analytics";
            public const string ViewEngagement = Default + ".ViewEngagement";
            public const string ViewPerformance = Default + ".ViewPerformance";
            public const string ViewNetworkInsights = Default + ".ViewNetworkInsights";
            public const string ExportReports = Default + ".ExportReports";
        }
    }
}
