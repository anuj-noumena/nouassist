namespace NouAssist.Approval.Enums
{
    /// <summary>
    /// Defines the type of actions that can be associated with an approval step.
    /// These are the allowable or automatic actions in a workflow definition.
    /// </summary>
    public enum ApprovalActionType : byte
    {
        /// <summary>
        /// Action to approve a request.
        /// </summary>
        Approve = 1,

        /// <summary>
        /// Action to reject a request.
        /// </summary>
        Reject = 2,

        /// <summary>
        /// Action to delegate the approval to another user.
        /// </summary>
        Delegate = 3,

        /// <summary>
        /// Action to escalate the approval to a higher authority.
        /// </summary>
        Escalate = 4,

        /// <summary>
        /// Automatic approval action (e.g., triggered on timeout or by rule).
        /// </summary>
        AutoApprove = 5,

        /// <summary>
        /// Automatic rejection action (e.g., triggered on timeout or by rule).
        /// </summary>
        AutoReject = 6,

        /// <summary>
        /// Action to abstain from making a decision.
        /// </summary>
        Abstain = 7,

        /// <summary>
        /// Action to recall or retract a previous decision.
        /// </summary>
        Recall = 8
    }
}
