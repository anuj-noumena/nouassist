using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp.Timing;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services
{
    /// <summary>
    /// Implementation of the automation workflow domain service
    /// </summary>
    public class AutomationManager : DomainService, IAutomationManager
    {
        private readonly IAutomationWorkflowRepository _workflowRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IClock _clock;
        private readonly ILogger<AutomationManager> _logger;

        public AutomationManager(
            IAutomationWorkflowRepository workflowRepository,
            IGuidGenerator guidGenerator,
            IClock clock,
            ILogger<AutomationManager> logger)
        {
            _workflowRepository = workflowRepository;
            _guidGenerator = guidGenerator;
            _clock = clock;
            _logger = logger;
        }

        public async Task<AutomationWorkflow> CreateWorkflowAsync(
            string name,
            string description,
            AutomationTriggerType triggerType,
            string entityType,
            string fieldName = null,
            string expectedValue = null,
            string scheduleExpression = null,
            bool activate = false)
        {
            // Create trigger condition value object
            var triggerCondition = new AutomationTriggerCondition(
                triggerType,
                entityType,
                fieldName,
                expectedValue,
                scheduleExpression);
                
            // Create workflow
            var workflow = AutomationWorkflow.Create(
                _guidGenerator.Create(),
                name,
                description,
                triggerCondition,
                activate, // Default to inactive unless explicitly activated
                CurrentTenant.Id);
                
            return await _workflowRepository.InsertAsync(workflow, autoSave: true);
        }

        public async Task<AutomationWorkflow> UpdateWorkflowAsync(
            Guid id,
            string name = null,
            string description = null)
        {
            var workflow = await _workflowRepository.GetAsync(id);
            
            if (name != null)
            {
                workflow.UpdateName(name);
            }
            
            if (description != null)
            {
                workflow.UpdateDescription(description);
            }
            
            return await _workflowRepository.UpdateAsync(workflow, autoSave: true);
        }

        public async Task<AutomationWorkflow> UpdateTriggerConditionAsync(
            Guid id,
            AutomationTriggerType? triggerType = null,
            string entityType = null,
            string fieldName = null,
            string expectedValue = null,
            string scheduleExpression = null)
        {
            var workflow = await _workflowRepository.GetAsync(id);
            
            // Get current values for properties that are not being updated
            var currentTrigger = workflow.TriggerCondition;
            
            // Create new trigger condition with updated values
            var newTriggerCondition = new AutomationTriggerCondition(
                triggerType ?? currentTrigger.TriggerType,
                entityType ?? currentTrigger.EntityType,
                fieldName ?? currentTrigger.FieldName,
                expectedValue ?? currentTrigger.ExpectedValue,
                scheduleExpression ?? currentTrigger.ScheduleExpression);
                
            workflow.UpdateTriggerCondition(newTriggerCondition);
            
            return await _workflowRepository.UpdateAsync(workflow, autoSave: true);
        }

        public async Task<AutomationWorkflow> ActivateWorkflowAsync(
            Guid id)
        {
            var workflow = await _workflowRepository.GetAsync(id);
            
            if (workflow.IsActive)
            {
                throw new BusinessException("CRM:ErrorMessage:WorkflowAlreadyActive");
            }
            
            workflow.Activate();
            return await _workflowRepository.UpdateAsync(workflow, autoSave: true);
        }

        public async Task<AutomationWorkflow> DeactivateWorkflowAsync(
            Guid id)
        {
            var workflow = await _workflowRepository.GetAsync(id);
            workflow.Deactivate();
            return await _workflowRepository.UpdateAsync(workflow, autoSave: true);
        }

        public async Task<WorkflowStep> AddWorkflowStepAsync(
            Guid workflowId,
            string name,
            string description,
            AutomationActionType actionType,
            string actionConfiguration,
            TimeSpan? delayBefore = null)
        {
            var workflow = await _workflowRepository.GetAsync(workflowId);
            
            var step = workflow.AddStep(
                name,
                description,
                actionType,
                actionConfiguration,
                delayBefore);
                
            await _workflowRepository.UpdateAsync(workflow, autoSave: true);
            
            return step;
        }

        public async Task<WorkflowStep> UpdateWorkflowStepAsync(
            Guid workflowId,
            WorkflowStep step,
            string name = null,
            string description = null,
            AutomationActionType? actionType = null,
            string actionConfiguration = null,
            TimeSpan? delayBefore = null)
        {
            var workflow = await _workflowRepository.GetAsync(workflowId);
            
            workflow.UpdateStep(
                step,
                name,
                description,
                actionType,
                actionConfiguration,
                delayBefore);
                
            await _workflowRepository.UpdateAsync(workflow, autoSave: true);
            
            return step;
        }

        public async Task ReorderWorkflowStepsAsync(
            Guid workflowId,
            List<(WorkflowStep Step, int NewOrder)> newOrdering)
        {
            var workflow = await _workflowRepository.GetAsync(workflowId);
            workflow.ReorderSteps(newOrdering);
            await _workflowRepository.UpdateAsync(workflow, autoSave: true);
        }

        public async Task<WorkflowStep> EnableWorkflowStepAsync(
            Guid workflowId,
            WorkflowStep step)
        {
            var workflow = await _workflowRepository.GetAsync(workflowId);
            workflow.EnableStep(step);
            await _workflowRepository.UpdateAsync(workflow, autoSave: true);
            return step;
        }

        public async Task<WorkflowStep> DisableWorkflowStepAsync(
            Guid workflowId,
            WorkflowStep step)
        {
            var workflow = await _workflowRepository.GetAsync(workflowId);
            workflow.DisableStep(step);
            await _workflowRepository.UpdateAsync(workflow, autoSave: true);
            return step;
        }

        public async Task RemoveWorkflowStepAsync(
            Guid workflowId,
            WorkflowStep step)
        {
            var workflow = await _workflowRepository.GetAsync(workflowId);
            workflow.RemoveStep(step);
            await _workflowRepository.UpdateAsync(workflow, autoSave: true);
        }

        public async Task<WorkflowExecutionRecord> TriggerWorkflowExecutionAsync(
            Guid workflowId,
            Guid triggeredByEntityId,
            string triggeredByEntityType)
        {
            var workflow = await _workflowRepository.GetAsync(workflowId);
            
            if (!workflow.IsActive)
            {
                throw new BusinessException("CRM:ErrorMessage:WorkflowNotActive");
            }
            
            var executionRecord = workflow.RecordExecution(
                _guidGenerator.Create(),
                triggeredByEntityId,
                triggeredByEntityType);
                
            await _workflowRepository.UpdateAsync(workflow, autoSave: true);
            
            _logger.LogInformation("Workflow {WorkflowId} triggered for {EntityType} {EntityId}", 
                workflowId, triggeredByEntityType, triggeredByEntityId);
                
            // In a real implementation, we would start the execution process here
            // This might involve a background job or calling a workflow engine
            
            return executionRecord;
        }

        public async Task<List<AutomationWorkflow>> GetMatchingWorkflowsAsync(
            AutomationTriggerType triggerType,
            string entityType,
            string fieldName = null,
            string fieldValue = null)
        {
            if (triggerType == AutomationTriggerType.CustomField || 
                triggerType == AutomationTriggerType.LeadStatusChanged)
            {
                if (string.IsNullOrEmpty(fieldName))
                {
                    return new List<AutomationWorkflow>();
                }
                
                return await _workflowRepository.GetByFieldTriggerAsync(
                    entityType,
                    fieldName,
                    fieldValue,
                    activeOnly: true);
            }
            else if (triggerType == AutomationTriggerType.ScheduledTime)
            {
                return await _workflowRepository.GetScheduledWorkflowsAsync(activeOnly: true);
            }
            else
            {
                return await _workflowRepository.GetByTriggerTypeAsync(
                    triggerType,
                    entityType,
                    activeOnly: true);
            }
        }
    }
}
