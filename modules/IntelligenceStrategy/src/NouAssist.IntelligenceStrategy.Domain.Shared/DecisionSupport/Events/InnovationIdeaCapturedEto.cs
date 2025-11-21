using System;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using NouAssist.IntelligenceStrategy.DecisionSupport.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Events
{
    /// <summary>
    /// Event raised when an innovation idea is captured
    /// </summary>
    [EventName("NouAssist.DecisionSupport.InnovationIdeaCaptured")]
    public class InnovationIdeaCapturedEto : DecisionSupportEtoBase
    {
        /// <summary>
        /// Title of the innovation idea
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Description of the innovation idea
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Current stage of the idea
        /// </summary>
        public IdeaStage Stage { get; set; }
        
        /// <summary>
        /// Date the idea was captured
        /// </summary>
        public DateTime CapturedDate { get; set; }
        
        /// <summary>
        /// User ID of the person who captured the idea
        /// </summary>
        public Guid CapturedBy { get; set; }
    }
}
