namespace NouAssist.RequirementsManagement.QualityAssurance.Permissions
{
    public static class QAPermissions
    {
        public const string GroupName = "QA";

        // Permissions for TestPlan Management
        public static class TestPlan
        {
            public const string Default = GroupName + ".TestPlan"; // Group permission
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View"; // Permission to view details and lists
            public const string Activate = Default + ".Activate";
            public const string Complete = Default + ".Complete";
            public const string Archive = Default + ".Archive";
        }

        // Permissions for TestCycle Management
        public static class TestCycle
        {
            public const string Default = GroupName + ".TestCycle";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Start = Default + ".Start";
            public const string Complete = Default + ".Complete";
            public const string ManageTestRuns = Default + ".ManageTestRuns";
            public const string RecordResults = Default + ".RecordResults";
        }

        // Permissions for Defect Management
        public static class Defect
        {
            public const string Default = GroupName + ".Defect";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Assign = Default + ".Assign";
            public const string ChangeStatus = Default + ".ChangeStatus";
            public const string VerifyFix = Default + ".VerifyFix";
            public const string Comment = Default + ".Comment";
        }

        // Permissions for Quality Reporting
        public static class Report
        {
            public const string Default = GroupName + ".Report";
            public const string View = Default + ".View";
            public const string Export = Default + ".Export";
            public const string ManageMetrics = Default + ".ManageMetrics";
        }
    }
}
