using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Exceptions;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates
{
    /// <summary>
    /// Aggregate root representing a personal automation defined by a user
    /// </summary>
    public class PersonalAutomation : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// ID of the user this automation belongs to
        /// </summary>
        public Guid UserId { get; protected set; }
        
        /// <summary>
        /// Name of the automation
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of what the automation does
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Whether the automation is enabled
        /// </summary>
        public bool IsEnabled { get; private set; }
        
        /// <summary>
        /// Triggers that can activate this automation
        /// </summary>
        private readonly List<AutomationTrigger> _triggers;
        
        /// <summary>
        /// Actions to perform when triggered
        /// </summary>
        private readonly List<AutomationAction> _actions;
        
        /// <summary>
        /// Public access to triggers
        /// </summary>
        public IReadOnlyCollection<AutomationTrigger> Triggers => _triggers.AsReadOnly();
        
        /// <summary>
        /// Public access to actions
        /// </summary>
        public IReadOnlyCollection<AutomationAction> Actions => _actions.AsReadOnly();

        protected PersonalAutomation()
        {
            _triggers = new List<AutomationTrigger>();
            _actions = new List<AutomationAction>();
        }

        /// <summary>
        /// Creates a new personal automation
        /// </summary>
        public static PersonalAutomation Create(
            Guid id,
            Guid userId,
            string name,
            string description,
            bool isEnabled = true,
            Guid? tenantId = null)
        {
            return new PersonalAutomation(id, userId, name, description, isEnabled, tenantId);
        }

        private PersonalAutomation(
            Guid id,
            Guid userId,
            string name,
            string description,
            bool isEnabled,
            Guid? tenantId) : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            SetName(name);
            SetDescription(description);
            IsEnabled = isEnabled;
            
            _triggers = new List<AutomationTrigger>();
            _actions = new List<AutomationAction>();
        }

        /// <summary>
        /// Sets the name of the automation
        /// </summary>
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: PersonalProductivityIntelligenceConsts.MaxNameLength);
        }

        /// <summary>
        /// Sets the description of the automation
        /// </summary>
        public void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: PersonalProductivityIntelligenceConsts.MaxDescriptionLength);
        }

        /// <summary>
        /// Enables the automation
        /// </summary>
        public void Enable()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// Disables the automation
        /// </summary>
        public void Disable()
        {
            IsEnabled = false;
        }

        /// <summary>
        /// Adds a trigger to the automation
        /// </summary>
        public void AddTrigger(
            AutomationTriggerType triggerType,
            string triggerData,
            string triggerDescription = null)
        {
            if (_triggers.Count >= PersonalProductivityIntelligenceConsts.MaxAutomationTriggersCount)
            {
                throw new PersonalProductivityIntelligenceDomainException(
                    "PersonalProductivityIntelligence:ErrorMessage:ExceedsMaxTriggersCount")
                    .WithData("maxCount", PersonalProductivityIntelligenceConsts.MaxAutomationTriggersCount);
            }
            
            var trigger = new AutomationTrigger(
                GuidGenerator.Create(),
                Id,
                triggerType,
                triggerData,
                triggerDescription);
            
            _triggers.Add(trigger);
        }

        /// <summary>
        /// Removes a trigger from the automation
        /// </summary>
        public void RemoveTrigger(Guid triggerId)
        {
            var trigger = _triggers.FirstOrDefault(t => t.Id == triggerId);
            if (trigger != null)
            {
                _triggers.Remove(trigger);
            }
        }

        /// <summary>
        /// Adds an action to the automation
        /// </summary>
        public void AddAction(
            string actionType,
            string actionData,
            int order,
            string actionDescription = null)
        {
            if (_actions.Count >= PersonalProductivityIntelligenceConsts.MaxAutomationActionsCount)
            {
                throw new PersonalProductivityIntelligenceDomainException(
                    "PersonalProductivityIntelligence:ErrorMessage:ExceedsMaxActionsCount")
                    .WithData("maxCount", PersonalProductivityIntelligenceConsts.MaxAutomationActionsCount);
            }
            
            var action = new AutomationAction(
                GuidGenerator.Create(),
                Id,
                actionType,
                actionData,
                order,
                actionDescription);
            
            _actions.Add(action);
            
            // Ensure actions are ordered
            ReorderActions();
        }

        /// <summary>
        /// Removes an action from the automation
        /// </summary>
        public void RemoveAction(Guid actionId)
        {
            var action = _actions.FirstOrDefault(a => a.Id == actionId);
            if (action != null)
            {
                _actions.Remove(action);
                ReorderActions();
            }
        }

        /// <summary>
        /// Changes the order of an action
        /// </summary>
        public void ChangeActionOrder(Guid actionId, int newOrder)
        {
            var action = _actions.FirstOrDefault(a => a.Id == actionId);
            if (action != null)
            {
                action.SetOrder(newOrder);
                ReorderActions();
            }
        }

        /// <summary>
        /// Records that this automation was triggered
        /// </summary>
        public void RecordTriggered(
            AutomationTriggerType triggerType,
            string triggerDescription,
            DateTime triggeredAt)
        {
            if (!IsEnabled)
            {
                return;
            }
            
            // Publish event
            AddDistributedEvent(new PersonalAutomationTriggeredEto
            {
                UserId = UserId,
                TenantId = TenantId,
                AutomationId = Id,
                AutomationName = Name,
                TriggerType = triggerType,
                TriggerDescription = triggerDescription,
                Timestamp = triggeredAt
            });
        }

        /// <summary>
        /// Validates that the automation is properly configured
        /// </summary>
        public void Validate()
        {
            if (!_triggers.Any())
            {
                throw new InvalidAutomationDefinitionException(
                    Id, "Automation must have at least one trigger");
            }
            
            if (!_actions.Any())
            {
                throw new InvalidAutomationDefinitionException(
                    Id, "Automation must have at least one action");
            }
        }

        private void ReorderActions()
        {
            int order = 1;
            foreach (var action in _actions.OrderBy(a => a.Order))
            {
                action.SetOrder(order++);
            }
        }
    }
}
