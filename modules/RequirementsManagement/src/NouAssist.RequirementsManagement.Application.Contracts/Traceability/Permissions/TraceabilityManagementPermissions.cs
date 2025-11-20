namespace NouAssist.RequirementsManagement.Traceability.Permissions
{
    /// <summary>
    /// Permission definitions for TraceabilityManagement module
    /// </summary>
    public static class TraceabilityManagementPermissions
    {
        public const string GroupName = "TraceabilityManagement";
        
        public static class TraceLinks
        {
            public const string Default = GroupName + ".TraceLinks";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Verify = Default + ".Verify";
        }
        
        public static class TraceMatrices
        {
            public const string Default = GroupName + ".TraceMatrices";
            public const string Generate = Default + ".Generate";
            public const string CheckCoverage = Default + ".CheckCoverage";
        }
        
        public static class ImpactAnalysis
        {
            public const string Default = GroupName + ".ImpactAnalysis";
            public const string Analyze = Default + ".Analyze";
        }
        
        public static class ArtifactRegistry
        {
            public const string Default = GroupName + ".ArtifactRegistry";
            public const string Register = Default + ".Register";
            public const string Verify = Default + ".Verify";
        }
    }
}
