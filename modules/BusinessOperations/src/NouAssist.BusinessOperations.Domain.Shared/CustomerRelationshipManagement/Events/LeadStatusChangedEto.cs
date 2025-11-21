using System;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Events
{
    /// <summary>
    /// Event raised when a lead's status is changed
    /// </summary>
    [EventName("NouAssist.BusinessOperations.CustomerRelationshipManagement.LeadStatusChanged")]
    public class LeadStatusChangedEto : CrmEtoBase
    {
        /// <summary>
        /// The name of the lead
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The previous status of the lead
        /// </summary>
        public LeadStatus PreviousStatus { get; set; }
        
        /// <summary>
        /// The new status of the lead
        /// </summary>
        public LeadStatus NewStatus { get; set; }
        
        /// <summary>
        /// The time when the status changed
        /// </summary>
        public DateTime StatusChangeTime { get; set; }
    }
}
