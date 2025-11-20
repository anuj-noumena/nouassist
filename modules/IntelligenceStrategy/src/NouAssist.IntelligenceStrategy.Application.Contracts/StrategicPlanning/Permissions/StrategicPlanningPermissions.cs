namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Permissions
{
    /// <summary>
    /// Permission definitions for Strategic Planning module
    /// </summary>
    public static class StrategicPlanningPermissions
    {
        public const string GroupName = "StrategicPlanning";

        // Strategic Goals
        public static class StrategicGoals
        {
            public const string Default = GroupName + ".StrategicGoals";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Approve = Default + ".Approve";
            public const string Activate = Default + ".Activate";
            public const string ManageTargets = Default + ".ManageTargets";
        }

        // Crisis Management
        public static class CrisisManagement
        {
            public const string Default = GroupName + ".CrisisManagement";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Activate = Default + ".Activate";
            public const string ManageProtocols = Default + ".ManageProtocols";
        }

        // Sustainable Growth
        public static class SustainableGrowth
        {
            public const string Default = GroupName + ".SustainableGrowth";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageEsgGoals = Default + ".ManageEsgGoals";
            public const string RecordMeasurements = Default + ".RecordMeasurements";
        }

        // Integrated Planning
        public static class IntegratedPlanning
        {
            public const string Default = GroupName + ".IntegratedPlanning";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageTimeline = Default + ".ManageTimeline";
            public const string BackwardPlanning = Default + ".BackwardPlanning";
        }

        // Opportunity Management
        public static class OpportunityManagement
        {
            public const string Default = GroupName + ".OpportunityManagement";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageFilters = Default + ".ManageFilters";
            public const string EvaluateOpportunities = Default + ".EvaluateOpportunities";
        }

        // Transition Planning
        public static class TransitionPlanning
        {
            public const string Default = GroupName + ".TransitionPlanning";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageCapabilities = Default + ".ManageCapabilities";
            public const string UpdateProgress = Default + ".UpdateProgress";
        }
    }
}
