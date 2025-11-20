namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Permissions
{
    public static class BusinessModelManagementPermissions
    {
        public const string GroupName = "BusinessModelManagement";

        public static class BusinessModelCanvas
        {
            public const string Default = GroupName + ".BusinessModelCanvas";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string CreateVersion = Default + ".CreateVersion";
            public const string ManageComponents = Default + ".ManageComponents";
            public const string ManageHypotheses = Default + ".ManageHypotheses";
        }

        public static class RevenueModel
        {
            public const string Default = GroupName + ".RevenueModel";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManagePricing = Default + ".ManagePricing";
            public const string ManageScenarios = Default + ".ManageScenarios";
            public const string SimulateRevenue = Default + ".SimulateRevenue";
            public const string ManageGtmStrategies = Default + ".ManageGtmStrategies";
        }

        public static class StrategicPivot
        {
            public const string Default = GroupName + ".StrategicPivot";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageLifecycle = Default + ".ManageLifecycle";
            public const string ManageImpactMetrics = Default + ".ManageImpactMetrics";
            public const string ViewAnalytics = Default + ".ViewAnalytics";
        }
    }
}
