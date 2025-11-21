using System;
using NouAssist.IntelligenceStrategy.DecisionSupport.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Events
{
    /// <summary>
    /// Event triggered when a strategic initiative is rejected
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiativeRejected")]
    public class StrategicInitiativeRejectedEto : StrategicInitiativeEtoBase
    {
        /// <summary>
        /// Reason for rejecting the initiative
        /// </summary>
        public string RejectionReason { get; set; }
        
        /// <summary>
        /// User ID of the rejector
        /// </summary>
        public Guid RejectorId { get; set; }
        
        /// <summary>
        /// Timestamp when the initiative was rejected
        /// </summary>
        public DateTime RejectedOn { get; set; }
    }
}
