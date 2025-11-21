using System;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities
{
    /// <summary>
    /// Represents a step in an automation workflow
    /// </summary>
    public class WorkflowStep : Entity
    {
        /// <summary>
        /// The workflow this step belongs to
        /// </summary>
        public Guid WorkflowId { get; private set; }
        
        /// <summary>
        /// Order of execution within the workflow
        /// </summary>
        public int Order { get; private set; }
        
        /// <summary>
        /// Name of the step
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of what this step does
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Type of action to perform
        /// </summary>
        public AutomationActionType ActionType { get; private set; }
        
        /// <summary>
        /// Configuration for the action in JSON format
        /// </summary>
        public string ActionConfiguration { get; private set; }
        
        /// <summary>
        /// Optional delay before executing this step
        /// </summary>
        public TimeSpan? DelayBefore { get; private set; }
        
        /// <summary>
        /// Whether this step is enabled
        /// </summary>
        public bool IsEnabled { get; private set; }

        protected WorkflowStep()
        {
        }

        internal WorkflowStep(
            Guid workflowId,
            int order,
            string name,
            string description,
            AutomationActionType actionType,
            string actionConfiguration,
            TimeSpan? delayBefore = null)
        {
            WorkflowId = workflowId;
            Order = order;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Description = description;
            ActionType = actionType;
            ActionConfiguration = Check.NotNullOrWhiteSpace(actionConfiguration, nameof(actionConfiguration));
            DelayBefore = delayBefore;
            IsEnabled = true;
        }

        internal void Update(
            string name = null,
            string description = null,
            AutomationActionType? actionType = null,
            string actionConfiguration = null,
            TimeSpan? delayBefore = null,
            int? order = null)
        {
            if (name != null)
            {
                Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            }

            if (description != null)
            {
                Description = description;
            }

            if (actionType.HasValue)
            {
                ActionType = actionType.Value;
            }

            if (actionConfiguration != null)
            {
                ActionConfiguration = Check.NotNullOrWhiteSpace(actionConfiguration, nameof(actionConfiguration));
            }

            if (delayBefore.HasValue || delayBefore == null)
            {
                DelayBefore = delayBefore;
            }

            if (order.HasValue)
            {
                Order = order.Value;
            }
        }

        internal void Enable()
        {
            IsEnabled = true;
        }

        internal void Disable()
        {
            IsEnabled = false;
        }

        public override object[] GetKeys()
        {
            return new object[] { WorkflowId, Order };
        }
    }
}
