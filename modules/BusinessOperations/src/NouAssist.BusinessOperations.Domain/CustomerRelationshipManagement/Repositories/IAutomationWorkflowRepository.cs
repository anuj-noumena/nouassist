using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories
{
    /// <summary>
    /// Repository interface for AutomationWorkflow aggregate
    /// </summary>
    public interface IAutomationWorkflowRepository : IRepository<AutomationWorkflow, Guid>
    {
        /// <summary>
        /// Gets workflows configured for the specified trigger type
        /// </summary>
        Task<List<AutomationWorkflow>> GetByTriggerTypeAsync(
            AutomationTriggerType triggerType, 
            string entityType, 
            bool activeOnly = true, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets workflows triggered by field changes
        /// </summary>
        Task<List<AutomationWorkflow>> GetByFieldTriggerAsync(
            string entityType, 
            string fieldName, 
            string fieldValue = null, 
            bool activeOnly = true, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets scheduled workflows
        /// </summary>
        Task<List<AutomationWorkflow>> GetScheduledWorkflowsAsync(
            bool activeOnly = true, 
            CancellationToken cancellationToken = default);
    }
}
