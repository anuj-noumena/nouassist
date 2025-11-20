namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Permissions
{
    /// <summary>
    /// Defines permissions for the Requirements Analysis Tools module
    /// </summary>
    public static class RequirementsAnalysisToolsPermissions
    {
        public const string GroupName = "RequirementsAnalysisTools";

        // Analyzed Requirements permissions
        public static class AnalyzedRequirements
        {
            public const string Default = GroupName + ".AnalyzedRequirements";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageAttributes = Default + ".ManageAttributes";
            public const string ManageRelationships = Default + ".ManageRelationships";
            public const string ManageImpactAssessment = Default + ".ManageImpactAssessment";
        }

        // Glossary Terms permissions
        public static class GlossaryTerms
        {
            public const string Default = GroupName + ".GlossaryTerms";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Approve = Default + ".Approve";
            public const string ManageRelatedTerms = Default + ".ManageRelatedTerms";
        }

        // Prioritization Activities permissions
        public static class PrioritizationActivities
        {
            public const string Default = GroupName + ".PrioritizationActivities";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Manage = Default + ".Manage";
            public const string Vote = Default + ".Vote";
            public const string ViewResults = Default + ".ViewResults";
        }
    }
}
