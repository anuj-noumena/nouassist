namespace NouAssist.RequirementsManagement.ChangeManagement.Enums
{
    /// <summary>
    /// Represents the decision made by an approver on a change request
    /// </summary>
    public enum ApprovalDecision
    {
        /// <summary>
        /// No decision has been made yet
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Change request is approved
        /// </summary>
        Approved = 1,

        /// <summary>
        /// Change request is rejected
        /// </summary>
        Rejected = 2,

        /// <summary>
        /// Approver requests more information before deciding
        /// </summary>
        RequestMoreInfo = 3
    }
}
