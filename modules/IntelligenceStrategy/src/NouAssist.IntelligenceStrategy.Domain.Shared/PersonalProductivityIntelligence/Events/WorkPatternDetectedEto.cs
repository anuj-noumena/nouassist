using System;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events
{
    /// <summary>
    /// Event emitted when a new work pattern is detected for a user
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.WorkPatternDetected")]
    public class WorkPatternDetectedEto : ProductivityIntelligenceEtoBase
    {
        /// <summary>
        /// ID of the work pattern model
        /// </summary>
        public Guid WorkPatternModelId { get; set; }
        
        /// <summary>
        /// Description of the detected pattern
        /// </summary>
        public string PatternDescription { get; set; }
        
        /// <summary>
        /// Confidence level of the pattern detection
        /// </summary>
        public double PatternConfidence { get; set; }
    }
}
