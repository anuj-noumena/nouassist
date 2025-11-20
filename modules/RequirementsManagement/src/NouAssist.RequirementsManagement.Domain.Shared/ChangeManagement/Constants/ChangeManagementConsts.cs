namespace NouAssist.RequirementsManagement.ChangeManagement.Constants
{
    /// <summary>
    /// Defines constants for the Change Management module
    /// </summary>
    public static class ChangeManagementConsts
    {
        /// <summary>
        /// Maximum length for change request titles
        /// </summary>
        public const int MaxTitleLength = 200;

        /// <summary>
        /// Maximum length for change request descriptions
        /// </summary>
        public const int MaxDescriptionLength = 2000;

        /// <summary>
        /// Maximum length for rationale text
        /// </summary>
        public const int MaxRationaleLength = 1000;

        /// <summary>
        /// Maximum length for decision reasons
        /// </summary>
        public const int MaxDecisionReasonLength = 500;

        /// <summary>
        /// Maximum length for approver role names
        /// </summary>
        public const int MaxApproverRoleLength = 100;

        /// <summary>
        /// Maximum length for request identifiers
        /// </summary>
        public const int MaxRequestIdentifierLength = 50;

        /// <summary>
        /// Maximum number of approval steps per change request
        /// </summary>
        public const int MaxApprovalSteps = 10;

        /// <summary>
        /// Default change request number prefix
        /// </summary>
        public const string DefaultRequestPrefix = "CR";
    }
}
