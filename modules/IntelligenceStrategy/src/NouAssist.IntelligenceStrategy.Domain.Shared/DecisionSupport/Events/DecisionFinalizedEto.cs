using System;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Events
{
    /// <summary>
    /// Event triggered when a decision is finalized
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.DecisionSupport.DecisionFinalized")]
    public class DecisionFinalizedEto
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
        /// ID of the selected alternative
        /// </summary>
        public Guid SelectedAlternativeId { get; set; }
        
        /// <summary>
        /// When the decision was finalized
        /// </summary>
        public DateTime DecisionDate { get; set; }
        
        /// <summary>
        /// The tenant ID if multi-tenancy is enabled
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
