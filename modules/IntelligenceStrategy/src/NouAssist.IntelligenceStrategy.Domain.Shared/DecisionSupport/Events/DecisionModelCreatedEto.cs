namespace NouAssist.IntelligenceStrategy.DecisionSupport.Events
{
    /// <summary>
    /// Event triggered when a decision model is created
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModelCreated")]
    public class DecisionModelCreatedEto
    {
        /// <summary>
        /// ID of the decision model
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Title of the decision model
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Type of decision model
        /// </summary>
        public string ModelType { get; set; }
        
        /// <summary>
        /// The tenant ID if multi-tenancy is enabled
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
