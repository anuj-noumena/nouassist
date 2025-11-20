namespace NouAssist.BusinessOperations.FreelanceOperations.Events
{
    /// <summary>
    /// Event raised when a new proposal is created
    /// </summary>
    [EventName("NouAssist.BusinessOperations.FreelanceOperations.ProposalCreated")]
    public class ProposalCreatedEto : ProposalEtoBase
    {
        /// <summary>
        /// Summary of the proposal content
        /// </summary>
        public string Summary { get; set; }
        
        /// <summary>
        /// Creation date of the proposal
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
