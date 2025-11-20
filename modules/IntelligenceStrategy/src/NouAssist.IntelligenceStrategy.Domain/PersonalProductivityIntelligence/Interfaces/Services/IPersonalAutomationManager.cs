using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Interfaces.Services
{
    /// <summary>
    /// Defines operations for managing personal automations
    /// </summary>
    public interface IPersonalAutomationManager
    {
        /// <summary>
        /// Creates a new personal automation
        /// </summary>
        Task<PersonalAutomation> CreateAutomationAsync(
            Guid userId,
            string name,
            string description,
            bool isEnabled = true);
        
        /// <summary>
        /// Updates an existing automation
        /// </summary>
        Task<PersonalAutomation> UpdateAutomationAsync(
            Guid automationId,
            string name,
            string description,
            bool isEnabled);
        
        /// <summary>
        /// Adds a trigger to an automation
        /// </summary>
        Task AddTriggerAsync(
            Guid automationId,
            AutomationTriggerType triggerType,
            string triggerData,
            string triggerDescription = null);
        
        /// <summary>
        /// Adds an action to an automation
        /// </summary>
        Task AddActionAsync(
            Guid automationId,
            string actionType,
            string actionData,
            int order,
            string actionDescription = null);
        
        /// <summary>
        /// Executes an automation manually
        /// </summary>
        Task ExecuteAutomationAsync(Guid automationId);
        
        /// <summary>
        /// Checks triggers and executes automations
        /// </summary>
        Task ProcessTriggersAsync(
            Guid userId,
            AutomationTriggerType triggerType,
            Dictionary<string, string> contextData);
    }
}
