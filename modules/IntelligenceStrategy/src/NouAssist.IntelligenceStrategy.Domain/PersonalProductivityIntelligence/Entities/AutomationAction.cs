using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities
{
    /// <summary>
    /// Represents an action for a personal automation
    /// </summary>
    public class AutomationAction : Entity<Guid>
    {
        /// <summary>
        /// ID of the automation this action belongs to
        /// </summary>
        public Guid AutomationId { get; protected set; }
        
        /// <summary>
        /// Type of action (e.g., "CreateTask", "SendEmail", "UpdateStatus")
        /// </summary>
        public string ActionType { get; protected set; }
        
        /// <summary>
        /// Data defining the action (JSON or structured format)
        /// </summary>
        public string ActionData { get; protected set; }
        
        /// <summary>
        /// Order in which to execute actions
        /// </summary>
        public int Order { get; protected set; }
        
        /// <summary>
        /// Human-readable description of the action
        /// </summary>
        public string ActionDescription { get; protected set; }

        protected AutomationAction()
        {
        }

        /// <summary>
        /// Creates a new automation action
        /// </summary>
        public AutomationAction(
            Guid id,
            Guid automationId,
            string actionType,
            string actionData,
            int order,
            string actionDescription) : base(id)
        {
            AutomationId = automationId;
            ActionType = actionType;
            ActionData = actionData;
            Order = order;
            ActionDescription = actionDescription;
        }

        /// <summary>
        /// Sets the execution order of this action
        /// </summary>
        public void SetOrder(int order)
        {
            Order = order;
        }
    }
}
