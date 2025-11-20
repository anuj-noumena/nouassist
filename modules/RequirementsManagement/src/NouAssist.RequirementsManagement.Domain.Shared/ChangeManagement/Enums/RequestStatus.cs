namespace NouAssist.RequirementsManagement.ChangeManagement.Enums
{
    /// <summary>
    /// Represents the status of a change request throughout its lifecycle
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        /// Change request has been submitted but not yet reviewed
        /// </summary>
        Submitted = 0,

        /// <summary>
        /// Change request is under review by assignees
        /// </summary>
        UnderReview = 1,

        /// <summary>
        /// Impact assessment is being conducted
        /// </summary>
        ImpactAssessment = 2,

        /// <summary>
        /// Awaiting approval from designated approvers
        /// </summary>
        PendingApproval = 3,

        /// <summary>
        /// Change request has been approved
        /// </summary>
        Approved = 4,

        /// <summary>
        /// Change request has been rejected
        /// </summary>
        Rejected = 5,

        /// <summary>
        /// Implementation of the approved change is in progress
        /// </summary>
        InProgress = 6,

        /// <summary>
        /// Change has been successfully implemented
        /// </summary>
        Completed = 7,

        /// <summary>
        /// Change request has been cancelled
        /// </summary>
        Cancelled = 8
    }
}
