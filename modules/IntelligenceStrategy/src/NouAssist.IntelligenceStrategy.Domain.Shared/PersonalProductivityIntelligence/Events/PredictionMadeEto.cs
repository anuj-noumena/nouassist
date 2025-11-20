using System;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events
{
    /// <summary>
    /// Event emitted when a prediction is made for a user
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.PredictionMade")]
    public class PredictionMadeEto : ProductivityIntelligenceEtoBase
    {
        /// <summary>
        /// ID of the prediction instance
        /// </summary>
        public Guid PredictionId { get; set; }
        
        /// <summary>
        /// Type of prediction
        /// </summary>
        public string PredictionType { get; set; }
        
        /// <summary>
        /// Confidence level of the prediction
        /// </summary>
        public PredictionConfidenceLevel ConfidenceLevel { get; set; }
        
        /// <summary>
        /// Target entity ID that the prediction is about (task, project, etc.)
        /// </summary>
        public Guid? TargetEntityId { get; set; }
        
        /// <summary>
        /// Type of the target entity
        /// </summary>
        public string TargetEntityType { get; set; }
    }
}
