using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Data;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Entities;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Events;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Services.Conditions;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;


namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Services

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

        private readonly ITransitionPermissionPolicyRepository _permissionPolicyRepository;


        public WorkflowTransitionService(
            WorkflowActionExecutorService actionExecutor,
            WorkflowConditionEvaluationService conditionEvaluationService,
            ITransitionPermissionPolicyRepository permissionPolicyRepository)

        {
            _actionExecutor = actionExecutor;

            _conditionEvaluationService = conditionEvaluationService;

            _permissionPolicyRepository = permissionPolicyRepository;
        }

        public async Task<TransitionExecutionResult> ExecuteTransitionAsync(
            Guid taskInstanceId,
            Aggregates.Workflow workflow,
            WorkflowTransition transition,
            Guid currentUserId,
            List<string> userRoles,
            Guid? projectId = null,
            Dictionary<string, object> taskCustomFields = null,
            string comment = null)

        {
            var result = new TransitionExecutionResult { IsSuccessful = true };


            // Validate transition (includes condition evaluation)

            var validationResult = await ValidateTransitionAsync(
                workflow, transition, currentUserId, userRoles, projectId, taskCustomFields, comment);
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
            Guid? projectId = null,
            Dictionary<string, object> taskCustomFields = null,
            string comment = null)

        {
            var result = new TransitionValidationResult { IsValid = true };

            // Validate user permissions using the new permission policy aggregate
            var hasPermission = await CheckUserPermissionAsync(
                transition.Id,
                currentUserId,
                userRoles,
                projectId);


            if (!hasPermission)
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


        /// <summary>
        /// Checks if a user has permission to perform a transition based on permission policies.
        /// Returns true if no policy exists (allow all) or if user matches policy criteria.
        /// </summary>
        private async Task<bool> CheckUserPermissionAsync(
            Guid transitionId,
            Guid userId,
            List<string> userRoles,
            Guid? projectId)
        {
            var policy = await _permissionPolicyRepository.GetApplicablePolicyAsync(
                transitionId,
                projectId);
            
            // No policy = allow all (default behavior)
            if (policy == null)
            {
                return true;
            }
            
            // Use policy's permission checking logic
            return policy.CanUserPerform(userId, userRoles);
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