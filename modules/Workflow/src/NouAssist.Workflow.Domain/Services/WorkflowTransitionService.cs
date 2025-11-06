using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Workflow.Aggregates;
using NouAssist.Workflow.Entities;
using NouAssist.Workflow.Events;
using NouAssist.Workflow.Services.Conditions;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.Workflow.Services
{
    /// <summary>
    /// Service for managing workflow transitions with condition evaluation.
    /// Follows Single Responsibility Principle - handles transition coordination
    /// Follows Dependency Inversion Principle - depends on abstractions
    /// </summary>
    public class WorkflowTransitionService : DomainService
    {
        private readonly WorkflowActionExecutorService _actionExecutor;
        private readonly WorkflowConditionEvaluationService _conditionEvaluationService;

        public WorkflowTransitionService(
            WorkflowActionExecutorService actionExecutor,
            WorkflowConditionEvaluationService conditionEvaluationService)
        {
            _actionExecutor = actionExecutor;
            _conditionEvaluationService = conditionEvaluationService;
        }

        public async Task<TransitionExecutionResult> ExecuteTransitionAsync(
            Guid taskInstanceId,
            Aggregates.Workflow workflow,
            WorkflowTransition transition,
            Guid currentUserId,
            List<string> userRoles,
            Dictionary<string, object> taskCustomFields = null,
            string comment = null)
        {
            var result = new TransitionExecutionResult { IsSuccessful = true };

            // Validate transition (includes condition evaluation)
            var validationResult = await ValidateTransitionAsync(
                workflow, transition, currentUserId, userRoles, taskCustomFields, comment);
            if (!validationResult.IsValid)
            {
                result.IsSuccessful = false;
                result.Errors.AddRange(validationResult.Errors);
                return result;
            }

            // Get ordered actions
            var actions = GetOrderedActions(transition);

            // Create action context
            var actionContext = new WorkflowActionContext
            {
                TaskInstanceId = taskInstanceId,
                WorkflowId = workflow.Id,
                TransitionId = transition.Id,
                ExecutedBy = currentUserId,
                Comment = comment
            };

            // Execute actions
            var actionResult = await _actionExecutor.ExecuteActionsAsync(actions, actionContext);
            if (!actionResult.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = actionResult.ErrorMessage;
                return result;
            }

            result.ExecutedActionIds = actionResult.ExecutedActions;
            result.QueuedActionIds = actionResult.QueuedActions;

            // Create and return transition event
            result.TransitionEvent = CreateTransitionEvent(
                taskInstanceId,
                workflow,
                transition,
                currentUserId,
                CurrentTenant.Id,
                comment);

            return result;
        }

        public async Task<TransitionValidationResult> ValidateTransitionAsync(
            Aggregates.Workflow workflow,
            WorkflowTransition transition,
            Guid currentUserId,
            List<string> userRoles,
            Dictionary<string, object> taskCustomFields = null,
            string comment = null)
        {
            var result = new TransitionValidationResult { IsValid = true };

            // Validate user permissions
            if (!transition.CanUserPerform(currentUserId, userRoles))
            {
                result.IsValid = false;
                result.Errors.Add("User does not have permission to perform this transition.");
            }

            // Validate comment requirement
            if (transition.RequiresComment && string.IsNullOrWhiteSpace(comment))
            {
                result.IsValid = false;
                result.Errors.Add("This transition requires a comment.");
            }

            // Validate status exists
            try
            {
                workflow.GetStatus(transition.ToStatusId);
            }
            catch (BusinessException)
            {
                result.IsValid = false;
                result.Errors.Add("Target status does not exist.");
            }

            // Evaluate transition conditions
            if (transition.HasConditions)
            {
                var conditionContext = new WorkflowConditionContext
                {
                    CurrentUserId = currentUserId,
                    UserRoles = userRoles,
                    CustomFieldValues = taskCustomFields ?? new Dictionary<string, object>()
                };

                var conditionResult = await _conditionEvaluationService.EvaluateAllAsync(
                    transition.Conditions, conditionContext);

                if (!conditionResult.AllSatisfied)
                {
                    result.IsValid = false;
                    result.Errors.AddRange(conditionResult.FailureReasons);
                }
            }

            return result;
        }

        public WorkflowTransitionExecutedEto CreateTransitionEvent(
            Guid taskId,
            Aggregates.Workflow workflow,
            WorkflowTransition transition,
            Guid executedBy,
            Guid? tenantId,
            string comment = null)
        {
            return new WorkflowTransitionExecutedEto
            {
                TaskId = taskId,
                WorkflowId = workflow.Id,
                TransitionId = transition.Id,
                FromStatusId = transition.FromStatusId,
                ToStatusId = transition.ToStatusId,
                ExecutedBy = executedBy,
                TenantId = tenantId,
                ExecutedAt = Clock.Now,
                Comment = comment
            };
        }

        public List<WorkflowAction> GetOrderedActions(WorkflowTransition transition)
        {
            return transition.Actions
                .Where(a => a.IsEnabled)
                .OrderBy(a => a.OrderIndex)
                .ToList();
        }
    }

    public class TransitionValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class TransitionExecutionResult
    {
        public bool IsSuccessful { get; set; }
        public List<string> Errors { get; set; } = new();
        public string ErrorMessage { get; set; }
        public List<Guid> ExecutedActionIds { get; set; } = new();
        public List<Guid> QueuedActionIds { get; set; } = new();
        public WorkflowTransitionExecutedEto TransitionEvent { get; set; }
    }
}
