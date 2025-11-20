namespace NouAssist.IntelligenceStrategy.DecisionSupport.Events
{
    /// <summary>
    /// Event triggered when a strategic initiative is proposed
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiativeProposed")]
    public class StrategicInitiativeProposedEto : StrategicInitiativeEtoBase
    {
        /// <summary>
        /// Expected outcomes of the initiative
        /// </summary>
        public string ExpectedOutcomes { get; set; }
        
        /// <summary>
        /// User ID of the proposer
        /// </summary>
        public Guid ProposerId { get; set; }
    }
}
