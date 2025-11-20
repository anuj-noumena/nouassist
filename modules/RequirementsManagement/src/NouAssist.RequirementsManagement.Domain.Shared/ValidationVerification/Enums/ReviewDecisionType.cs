namespace NouAssist.RequirementsManagement.ValidationVerification.Enums
{
    /// <summary>
    /// Type of review decision made by a reviewer
    /// </summary>
    public enum ReviewDecisionType
    {
        /// <summary>
        /// No decision has been made yet
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Requirement is approved
        /// </summary>
        Approve = 1,

        /// <summary>
        /// Requirement is rejected
        /// </summary>
        Reject = 2,

        /// <summary>
        /// Changes are requested before approval
        /// </summary>
        RequestChanges = 3,

        /// <summary>
        /// Abstained from making a decision
        /// </summary>
        Abstain = 4
    }
}
