namespace NouAssist.RequirementsManagement.ValidationVerification.Permissions
{
    /// <summary>
    /// Permission constants for ValidationVerification module
    /// </summary>
    public static class ValidationVerificationPermissions
    {
        public const string GroupName = "ValidationVerification";

        public static class ReviewCycles
        {
            public const string Default = GroupName + ".ReviewCycles";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Start = Default + ".Start";
            public const string Cancel = Default + ".Cancel";
            public const string SubmitReview = Default + ".SubmitReview";
            public const string ManageReviewers = Default + ".ManageReviewers";
            public const string ViewAll = Default + ".ViewAll";
        }

        public static class TestCases
        {
            public const string Default = GroupName + ".TestCases";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Execute = Default + ".Execute";
            public const string ManageSteps = Default + ".ManageSteps";
            public const string ViewResults = Default + ".ViewResults";
            public const string ManageLinks = Default + ".ManageLinks";
        }

        public static class TestCoverage
        {
            public const string Default = GroupName + ".TestCoverage";
            public const string Analyze = Default + ".Analyze";
            public const string GenerateReports = Default + ".GenerateReports";
            public const string ViewReports = Default + ".ViewReports";
        }
    }
}
