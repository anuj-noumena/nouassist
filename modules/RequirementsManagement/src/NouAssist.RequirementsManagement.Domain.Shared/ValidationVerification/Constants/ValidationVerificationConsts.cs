namespace NouAssist.RequirementsManagement.ValidationVerification.Constants
{
    /// <summary>
    /// Constants for ValidationVerification module
    /// </summary>
    public static class ValidationVerificationConsts
    {
        public const string ModuleName = "ValidationVerification";
        public const string DbTablePrefix = "VV_";
        public const string DbSchema = null;

        public static class RequirementReviewCycle
        {
            public const int MaxNameLength = 256;
            public const int MaxDescriptionLength = 2000;
            public const int MaxCommentsLength = 4000;
        }

        public static class TestCase
        {
            public const int MaxNameLength = 256;
            public const int MaxDescriptionLength = 2000;
            public const int MaxPreconditionsLength = 1000;
            public const int MaxPostconditionsLength = 1000;
        }

        public static class TestStep
        {
            public const int MaxInstructionLength = 1000;
            public const int MaxExpectedResultLength = 1000;
            public const int MaxActualResultLength = 1000;
            public const int MaxCommentsLength = 500;
        }

        public static class ReviewFeedback
        {
            public const int MaxCommentLength = 2000;
            public const int MaxReasonLength = 500;
        }

        public static class WorkflowDefinition
        {
            public const int MaxNameLength = 256;
            public const int MaxVersionLength = 50;
        }
    }
}
