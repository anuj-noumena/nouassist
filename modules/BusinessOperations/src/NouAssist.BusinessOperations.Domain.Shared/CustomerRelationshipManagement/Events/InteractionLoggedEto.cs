using System;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Events
{
    /// <summary>
    /// Event raised when an interaction is logged
    /// </summary>
    [EventName("NouAssist.BusinessOperations.CustomerRelationshipManagement.InteractionLogged")]
    public class InteractionLoggedEto : CrmEtoBase
    {
        /// <summary>
        /// The ID of the related contact or lead
        /// </summary>
        public Guid RelatedEntityId { get; set; }
        
        /// <summary>
        /// Type of the entity this interaction is related to
        /// </summary>
        public string RelatedEntityType { get; set; }
        
        /// <summary>
        /// The type of interaction
        /// </summary>
        public InteractionType InteractionType { get; set; }
        
        /// <summary>
        /// The time when the interaction occurred
        /// </summary>
        public DateTime InteractionTime { get; set; }
        
        /// <summary>
        /// Brief summary of the interaction
        /// </summary>
        public string Summary { get; set; }
    }
}
