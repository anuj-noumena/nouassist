namespace NouAssist.BusinessOperations.FreelanceOperations.Enums
{
    /// <summary>
    /// Represents the status of a proposal
    /// </summary>
    public enum ProposalStatus
    {
        /// <summary>
        /// Proposal is in draft state
        /// </summary>
        Draft = 1,
        
        /// <summary>
        /// Proposal has been sent to the client
        /// </summary>
        Sent = 2,
        
        /// <summary>
        /// Proposal has been viewed by the client
        /// </summary>
        Viewed = 3,
        
        /// <summary>
        /// Proposal has been accepted by the client
        /// </summary>
        Accepted = 4,
        
        /// <summary>
        /// Proposal has been rejected by the client
        /// </summary>
        Rejected = 5,
        
        /// <summary>
        /// Proposal has expired
        /// </summary>
        Expired = 6,
        
        /// <summary>
        /// Proposal requires revision
        /// </summary>
        NeedsRevision = 7
    }
}
