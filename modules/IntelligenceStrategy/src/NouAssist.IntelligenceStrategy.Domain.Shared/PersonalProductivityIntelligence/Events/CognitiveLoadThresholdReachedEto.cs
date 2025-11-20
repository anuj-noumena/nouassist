using System;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events
{
    /// <summary>
    /// Event emitted when a user's cognitive load reaches a threshold
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.CognitiveLoadThresholdReached")]
    public class CognitiveLoadThresholdReachedEto : ProductivityIntelligenceEtoBase
    {
        /// <summary>
        /// Current cognitive load level
        /// </summary>
        public CognitiveLoadLevel LoadLevel { get; set; }
        
        /// <summary>
        /// Numeric value of the cognitive load (0-100)
        /// </summary>
        public double LoadValue { get; set; }
        
        /// <summary>
        /// Indicates if this is an increasing or decreasing threshold crossing
        /// </summary>
        public bool IsIncreasing { get; set; }
    }
}
