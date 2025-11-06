using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Workflow.Aggregates;
using NouAssist.Workflow.Entities;
using NouAssist.Workflow.Events;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.Workflow.Services
{
    public class WorkflowTransitionService : DomainService
    {
        private readonly WorkflowActionExecutorService _actionExecutor;

        public WorkflowTransitionService(WorkflowActionExecutorService actionExecutor)
        {
            _actionExecutor = actionExecutor;
        }

        public async Task<TransitionExecutionResult> ExecuteTransitionAsync(
            Guid taskInstanceId,
            Aggregates.Workflow workflow,
            WorkflowTransition transition,
            Guid currentUserId,
            List<string> userRoles,
            string comment = null)
        {
            var result = new TransitionExecutionResult { IsSuccessful = true };

            // Validate transition
            var validationResult = ValidateTransition(workflow, transition, currentUserId, userRoles, comment);
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

        public TransitionValidationResult ValidateTransition(
            Aggregates.Workflow workflow,
            WorkflowTransition transition,
            Guid currentUserId,
            List<string> userRoles,
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
