using System;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Events
{
    /// <summary>
    /// Event raised when an automation workflow is triggered
    /// </summary>
    [EventName("NouAssist.BusinessOperations.CustomerRelationshipManagement.AutomationWorkflowTriggered")]
    public class AutomationWorkflowTriggeredEto : CrmEtoBase
    {
        /// <summary>
        /// The name of the workflow that was triggered
        /// </summary>
        public string WorkflowName { get; set; }
        
        /// <summary>
        /// The type of trigger that initiated the workflow
        /// </summary>
        public AutomationTriggerType TriggerType { get; set; }
        
        /// <summary>
        /// The ID of the entity that triggered the workflow
        /// </summary>
        public Guid TriggeredByEntityId { get; set; }
        
        /// <summary>
        /// The type of the entity that triggered the workflow
        /// </summary>
        public string TriggeredByEntityType { get; set; }
        
        /// <summary>
        /// The time when the workflow was triggered
        /// </summary>
        public DateTime TriggerTime { get; set; }
    }
}
