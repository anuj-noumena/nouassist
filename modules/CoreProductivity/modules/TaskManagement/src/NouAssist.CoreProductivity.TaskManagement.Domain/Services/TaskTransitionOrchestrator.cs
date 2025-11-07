using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Tasks.Aggregates;
using NouAssist.Tasks.Data;
using NouAssist.Workflow.Data;
using NouAssist.Workflow.Services;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;

namespace NouAssist.Tasks.Services
{
    /// <summary>
    /// Orchestrates task transitions by integrating task state changes with workflow execution.
    /// Follows Facade Pattern - provides simplified interface to complex subsystems
    /// Follows Single Responsibility Principle - coordinates task and workflow operations
    /// Follows Dependency Inversion Principle - depends on abstractions
    /// </summary>
    public class TaskTransitionOrchestrator : DomainService
    {
        private readonly ITaskInstanceRepository _taskInstanceRepository;
        private readonly IWorkflowRepository _workflowRepository;
        private readonly WorkflowTransitionService _workflowTransitionService;
        private readonly IDistributedEventBus _distributedEventBus;

        private readonly ITransitionPermissionPolicyRepository _permissionPolicyRepository;


        public TaskTransitionOrchestrator(
            ITaskInstanceRepository taskInstanceRepository,
            IWorkflowRepository workflowRepository,
            WorkflowTransitionService workflowTransitionService,
            IDistributedEventBus distributedEventBus,
            ITransitionPermissionPolicyRepository permissionPolicyRepository)
        {
            _taskInstanceRepository = taskInstanceRepository;
            _workflowRepository = workflowRepository;
            _workflowTransitionService = workflowTransitionService;
            _distributedEventBus = distributedEventBus;
            _permissionPolicyRepository = permissionPolicyRepository;
        }

        /// <summary>
        /// Executes a task status transition with full workflow support:
        /// 1. Validates transition conditions
        /// 2. Updates task status
        /// 3. Executes post-transition actions
        /// 4. Publishes events
        /// </summary>
        public async Task<TaskTransitionResult> TransitionTaskAsync(
            Guid taskInstanceId,
            Guid transitionId,
            Guid currentUserId,
            List<string> userRoles,
            string comment = null)
        {
            var result = new TaskTransitionResult { IsSuccessful = false };

            try
            {
                // 1. Load task instance
                var taskInstance = await _taskInstanceRepository.GetAsync(taskInstanceId);
                result.TaskInstanceId = taskInstance.Id;
                result.OldStatusId = taskInstance.WorkflowStatusId;

                // 2. Load workflow and find transition
                var workflow = await _workflowRepository.GetAsync(taskInstance.WorkflowId);
                var transition = workflow.Transitions.FirstOrDefault(t => t.Id == transitionId);

                if (transition == null)
                {
                    result.ErrorMessage = $"Transition {transitionId} not found in workflow";
                    return result;
                }

                // 3. Validate that transition is from current status
                if (transition.FromStatusId != taskInstance.WorkflowStatusId)
                {
                    result.ErrorMessage =
                        $"Transition cannot be performed from current status {taskInstance.WorkflowStatusId}";
                    return result;
                }

                // 4. Execute workflow transition (validates conditions and executes actions)
                var transitionResult = await _workflowTransitionService.ExecuteTransitionAsync(
                    taskInstanceId,
                    workflow,
                    transition,
                    currentUserId,
                    userRoles,
                    taskInstance.ProjectId, // Pass project context for permission checking
                    taskInstance.CustomFieldValues,
                    comment);

                if (!transitionResult.IsSuccessful)
                {
                    result.ErrorMessage = transitionResult.ErrorMessage ?? "Transition validation failed";
                    result.Errors.AddRange(transitionResult.Errors);
                    return result;
                }

                // 5. Update task instance status
                taskInstance.UpdateWorkflowStatus(transition.ToStatusId);
                await _taskInstanceRepository.UpdateAsync(taskInstance);

                result.NewStatusId = taskInstance.WorkflowStatusId;
                result.ExecutedActionIds = transitionResult.ExecutedActionIds;
                result.QueuedActionIds = transitionResult.QueuedActionIds;
                result.IsSuccessful = true;

                // 6. Publish transition event
                if (transitionResult.TransitionEvent != null)
                {
                    await _distributedEventBus.PublishAsync(transitionResult.TransitionEvent);
                }

                Logger.LogInformation(
                    "Task instance {TaskInstanceId} transitioned from status {OldStatus} to {NewStatus} via transition {TransitionId}",
                    taskInstanceId, result.OldStatusId, result.NewStatusId, transitionId);

                return result;
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during task transition");
                result.ErrorMessage = ex.Message;
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected error during task transition");
                result.ErrorMessage = $"Task transition failed: {ex.Message}";
                return result;
            }
        }

        /// <summary>
        /// Gets available transitions for a task instance based on current status and user permissions
        /// </summary>
        public async Task<List<AvailableTransition>> GetAvailableTransitionsAsync(
            Guid taskInstanceId,
            Guid currentUserId,
            List<string> userRoles)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(taskInstanceId);
            var workflow = await _workflowRepository.GetAsync(taskInstance.WorkflowId);

            var availableTransitions = new List<AvailableTransition>();

            // Get transitions from current status
            var transitions = workflow.Transitions
                .Where(t => t.FromStatusId == taskInstance.WorkflowStatusId || t.IsGlobal)
                .OrderBy(t => t.OrderIndex);

            foreach (var transition in transitions)
            {
                // Check user permissions using the new permission policy aggregate

                var hasPermission = await CheckUserPermissionAsync(
                    transition.Id,
                    currentUserId,
                    userRoles,
                    taskInstance.ProjectId);


                if (!hasPermission)
                {
                    continue;
                }


                // Validate transition (including conditions)

                var validationResult = await _workflowTransitionService.ValidateTransitionAsync(
                    workflow,
                    transition,
                    currentUserId,
                    userRoles,
                    taskInstance.ProjectId, // Pass project context
                    taskInstance.CustomFieldValues);

                availableTransitions.Add(new AvailableTransition
                {
                    TransitionId = transition.Id,
                    TransitionName = transition.Name,
                    ToStatusId = transition.ToStatusId,
                    ButtonLabel = transition.ButtonLabel ?? transition.Name,
                    ButtonColor = transition.ButtonColor,
                    RequiresComment = transition.RequiresComment,
                    IsAvailable = validationResult.IsValid,
                    UnavailableReason = validationResult.IsValid
                        ? null
                        : string.Join(", ", validationResult.Errors)
                });
            }

            return availableTransitions;
        }


        /// <summary>
        /// Checks if a user has permission to perform a transition based on permission policies.
        /// Returns true if no policy exists (allow all) or if user matches policy criteria.
        /// </summary>
        private async Task<bool> CheckUserPermissionAsync(
            Guid transitionId,
            Guid userId,
            List<string> userRoles,
            Guid projectId)
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

    /// <summary>
    /// Result of a task transition operation
    /// </summary>
    public class TaskTransitionResult
    {
        public bool IsSuccessful { get; set; }
        public Guid TaskInstanceId { get; set; }
        public Guid? OldStatusId { get; set; }
        public Guid? NewStatusId { get; set; }
        public List<Guid> ExecutedActionIds { get; set; } = new();
        public List<Guid> QueuedActionIds { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Represents an available transition for a task
    /// </summary>
    public class AvailableTransition
    {
        public Guid TransitionId { get; set; }
        public string TransitionName { get; set; }
        public Guid ToStatusId { get; set; }
        public string ButtonLabel { get; set; }
        public string ButtonColor { get; set; }
        public bool RequiresComment { get; set; }
        public bool IsAvailable { get; set; }
        public string UnavailableReason { get; set; }
    }
}