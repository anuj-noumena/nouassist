using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Managers;
using NouAssist.WorkManagement.TaskManagement.Services;
using NouAssist.WorkManagement.TaskManagement.Specifications;
using NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using NouAssist.WorkManagement.Workflow.Data;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
using NouAssist.WorkManagement.Workflow.Services;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;
using Volo.Abp.Users;
using Task = System.Threading.Tasks.Task;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances
{
    [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Default)]
    public class TaskInstanceAppService : ApplicationService, ITaskInstanceAppService
    {
        private readonly ITaskInstanceRepository _taskInstanceRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly TaskInstanceManager _taskInstanceManager;
        private readonly TaskDependencyService _dependencyService;
        private readonly ICurrentUser _currentUser;

        public TaskInstanceAppService(
            ITaskInstanceRepository taskInstanceRepository,
            ITaskRepository taskRepository,
            TaskInstanceManager taskInstanceManager,
            TaskDependencyService dependencyService, ICurrentUser currentUser)
        {
            _taskInstanceRepository = taskInstanceRepository;
            _taskRepository = taskRepository;
            _taskInstanceManager = taskInstanceManager;
            _dependencyService = dependencyService;
            _currentUser = currentUser;
        }

        public async Task<TaskInstanceDto> GetAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);
            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        public async Task<TaskInstanceDetailDto> GetDetailAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);
            return ObjectMapper.Map<TaskInstance, TaskInstanceDetailDto>(taskInstance);
        }

        public async Task<PagedResultDto<TaskInstanceDto>> GetListAsync(GetTaskInstanceListDto input)
        {
            var spec = CreateListSpecification(input);

            var taskInstances = await _taskInstanceRepository.GetListAsync(spec, includeDetails: false);
            var totalCount = await _taskInstanceRepository.CountAsync(spec.ToExpression());

            // Apply sorting and paging
            var query = taskInstances.AsQueryable();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                query = query.OrderBy(input.Sorting);
            }
            else
            {
                query = query.OrderByDescending(ti => ti.CreationTime);
            }

            var pagedInstances = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<TaskInstanceDto>(
                totalCount,
                ObjectMapper.Map<List<TaskInstance>, List<TaskInstanceDto>>(pagedInstances)
            );
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Create)]
        public async Task<TaskInstanceDto> CreateAsync(CreateTaskInstanceDto input)
        {
            // Load the task blueprint
            var taskBlueprint = await _taskRepository.GetWithDetailsAsync(input.TaskId);

            // Use domain manager to create instance from blueprint
            var taskInstance = await _taskInstanceManager.CreateAsync(
                input.TaskId,
                input.WorkflowId,
                input.WorkflowStatusId,
                _currentUser.GetId(),
                input.ProjectId,
                input.AreaId,
                input.ContextId,
                input.ReporterId,
                input.AssigneeId,
                input.Importance,
                input.Urgency,
                _currentUser.TenantId
            );

            // Override priority if provided
            if (input.Importance.HasValue && input.Urgency.HasValue)
            {
                taskInstance.UpdatePriority(input.Importance.Value, input.Urgency.Value);
            }

            // Set planned dates if provided
            if (input.PlannedStartDate.HasValue || input.PlannedDueDate.HasValue)
            {
                taskInstance.SetPlannedDates(input.PlannedStartDate, input.PlannedDueDate);
            }

            // Set complexity estimation if provided (direct property assignment in new structure)
            if (input.ComplexityEstimation != null)
            {
                var complexityEstimation = new ComplexityEstimation(
                    input.ComplexityEstimation.Value,
                    input.ComplexityEstimation.Scale
                );
                // ComplexityEstimation is now a direct property, not set via method
                // TODO: Need to add method to TaskInstance aggregate for setting complexity
                // taskInstance.ComplexityEstimation = complexityEstimation;
            }

            // Set cognitive load if provided (direct property assignment in new structure)
            if (input.CognitiveLoad.HasValue)
            {
                // CognitiveLoad is now a direct property, not set via method
                // TODO: Need to add method to TaskInstance aggregate for setting cognitive load
                // taskInstance.CognitiveLoad = input.CognitiveLoad.Value;
            }

            // Set notes if provided
            if (!string.IsNullOrEmpty(input.Notes))
            {
                taskInstance.Notes = input.Notes;
            }

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> UpdateAsync(Guid id, UpdateTaskInstanceDto input)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            // Update planned dates using Schedule value object
            if (input.PlannedStartDate.HasValue || input.PlannedDueDate.HasValue)
            {
                taskInstance.SetPlannedDates(
                    input.PlannedStartDate ?? taskInstance.Schedule.PlannedStartDate,
                    input.PlannedDueDate ?? taskInstance.Schedule.PlannedDueDate
                );
            }

            // Update actual due date using Schedule value object
            if (input.ActualDueDate.HasValue)
            {
                var schedule = taskInstance.Schedule.WithActualDates(
                    taskInstance.Schedule.ActualStartDate,
                    input.ActualDueDate
                );
                // TODO: Need UpdateSchedule method in aggregate
                // taskInstance.Schedule = schedule;
            }

            if (input.Importance.HasValue && input.Urgency.HasValue)
            {
                taskInstance.UpdatePriority(input.Importance.Value, input.Urgency.Value);
            }

            // Update next action status using WorkTracking value object
            if (input.IsNextAction.HasValue)
            {
                if (input.IsNextAction.Value && !taskInstance.WorkTracking.IsNextAction)
                {
                    // TODO: Need to add MarkAsNextAction method that updates WorkTracking
                    // For now, this feature is not available in new structure
                }
                else if (!input.IsNextAction.Value && taskInstance.WorkTracking.IsNextAction)
                {
                    // TODO: Need to add UnmarkAsNextAction method that updates WorkTracking
                }
            }

            // Update flag status
            if (input.IsFlagged.HasValue)
            {
                if (input.IsFlagged.Value)
                {
                    taskInstance.Flag(input.FlaggedReason ?? "Flagged");
                }
                else
                {
                    taskInstance.Unflag();
                }
            }

            // Update complexity estimation if provided
            if (input.ComplexityEstimation != null)
            {
                // TODO: ComplexityEstimation needs setter method in aggregate
                // var complexityEstimation = new ComplexityEstimation(
                //     input.ComplexityEstimation.Value,
                //     input.ComplexityEstimation.Scale
                // );
            }

            // Update cognitive load if provided
            if (input.CognitiveLoad.HasValue)
            {
                // TODO: CognitiveLoad needs setter method in aggregate
            }

            // Update notes if provided
            if (input.Notes != null)
            {
                taskInstance.Notes = input.Notes;
            }

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _taskInstanceRepository.DeleteAsync(id);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Start)]
        public async Task<TaskInstanceDto> StartAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);
            taskInstance.Start();
            await _taskInstanceRepository.UpdateAsync(taskInstance);
            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Complete)]
        public async Task<TaskInstanceDto> CompleteAsync(Guid id, CompleteTaskDto input)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);
            taskInstance.Complete(input.Resolution, input.ResolutionNote);
            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> CancelAsync(Guid id, string reason)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            // TODO: Cancel method needs to be added to TaskInstance aggregate
            // For now, we'll block the task with the reason
            taskInstance.Block($"Cancelled: {reason}");

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> ChangeWorkflowStatusAsync(Guid id, ChangeWorkflowStatusDto input)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.TransitionTo(input.WorkflowStatusId, input.Comment);
            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> ReopenAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            if (!taskInstance.WorkTracking.IsCompleted)
            {
                throw new BusinessException("Tasks:CanOnlyReopenCompletedTasks");
            }

            // TODO: Need to add Reopen method to TaskInstance aggregate
            // For now, we unblock it if it was blocked
            if (taskInstance.IsBlocked)
            {
                taskInstance.Unblock();
            }

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        public Task<TaskInstanceDto> TransitionToAsync(Guid id, Guid workflowStatusId, string comment = null)
        {
            throw new NotImplementedException();
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> AssignAsync(Guid id, AssignTaskDto input)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.Assign(input.AssigneeId);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> UnassignAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);
            taskInstance.Unassign();
            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> ReassignAsync(Guid id, AssignTaskDto input)
        {
            return await AssignAsync(id, input);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Delegate)]
        public async Task<TaskInstanceDto> DelegateAsync(Guid id, DelegateTaskDto input)
        {
            // TODO: Delegation feature not yet implemented in new TaskInstance structure
            // Need to add delegation support to TaskInstance aggregate and TaskInstanceManager
            throw new NotImplementedException("Delegation feature is not yet implemented in the new structure");
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Delegate)]
        public async Task<TaskInstanceDto> DelegateViaWorkflowAsync(Guid id, DelegateTaskViaWorkflowDto input)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            // Get workflow for this task's project
            var workflowSchemeRepo = LazyServiceProvider.LazyGetRequiredService<IWorkflowSchemeRepository>();
            var workflowRepo = LazyServiceProvider.LazyGetRequiredService<IWorkflowRepository>();
            var workflowTransitionService = LazyServiceProvider.LazyGetRequiredService<WorkflowTransitionService>();

            var workflowScheme = await workflowSchemeRepo.GetActiveSchemeForProjectAsync(taskInstance.ProjectId.Value);
            if (workflowScheme == null)
            {
                throw new BusinessException("Tasks:NoWorkflowSchemeForProject",
                    $"No active workflow scheme found for project {taskInstance.ProjectId}");
            }

            // Check if quick delegation is allowed
            if (!workflowScheme.AllowQuickDelegation)
            {
                throw new BusinessException("Tasks:QuickDelegationNotAllowed",
                    "Quick delegation is not allowed for this project. Please use the standard delegation process.");
            }

            // Get the workflow for the task
            // TODO: Delegation via workflow not yet implemented in new structure
            throw new NotImplementedException("Delegation via workflow is not yet implemented");

            // The following code requires delegation support in TaskInstance aggregate:
            // var taskRepo = await LazyServiceProvider.LazyGetRequiredService<ITaskRepository>()
            //     .GetWithDetailsAsync(taskInstance.TaskId);
            // var workflowId = workflowScheme.GetWorkflowForTaskType(taskRepo.TaskTypeId);
            // var workflow = await workflowRepo.GetWithDetailsAsync(workflowId);
            // ... rest of delegation logic
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Delegate)]
        public async Task<TaskInstanceDto> AcceptDelegationAsync(Guid id, AcceptDelegationDto input)
        {
            // TODO: Delegation feature not yet implemented in new TaskInstance structure
            throw new NotImplementedException("Delegation feature is not yet implemented");
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Delegate)]
        public async Task<TaskInstanceDto> DeclineDelegationAsync(Guid id, DeclineDelegationDto input)
        {
            // TODO: Delegation feature not yet implemented in new TaskInstance structure
            throw new NotImplementedException("Delegation feature is not yet implemented");
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Delegate)]
        public async Task<TaskInstanceDto> RevokeDelegationAsync(Guid id)
        {
            // TODO: Delegation feature not yet implemented in new TaskInstance structure
            throw new NotImplementedException("Delegation feature is not yet implemented");
        }


        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Block)]
        public async Task<TaskInstanceDto> BlockAsync(Guid id, BlockTaskDto input)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.Block(input.Reason);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Block)]
        public async Task<TaskInstanceDto> UnblockAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.Unblock();

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Block)]
        public async Task<TaskInstanceDto> BlockWaitingForUserAsync(
            Guid id,
            string userName,
            string additionalContext = null)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            var reason = $"Waiting for {userName}";
            if (!string.IsNullOrEmpty(additionalContext))
            {
                reason += $": {additionalContext}";
            }
            taskInstance.Block(reason);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.UpdateProgress)]
        public async Task<TaskInstanceDto> CompleteStepAsync(Guid id, CompleteStepDto input)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);

            // Checklist items in new structure
            taskInstance.CheckItem(input.StepId, CurrentUser.Id);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.UpdateProgress)]
        public async Task<TaskInstanceDto> UncompleteStepAsync(Guid id, Guid stepId)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);

            // Checklist items in new structure
            taskInstance.UncheckItem(stepId);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.UpdateProgress)]
        public async Task<TaskInstanceDto> MarkCriterionAsMetAsync(Guid id, MarkCriterionMetDto input)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);

            taskInstance.AcceptanceCriteriaMarkAsMet(input.CriterionId, CurrentUser.Id.Value);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.UpdateProgress)]
        public async Task<TaskInstanceDto> UnmarkCriterionAsMetAsync(Guid id, Guid criterionId)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);

            var completion = taskInstance.CriterionCompletions
                .FirstOrDefault(cc => cc.CriterionId == criterionId);

            if (completion == null)
            {
                throw new BusinessException("Tasks:CriterionCompletionNotFound");
            }

            completion.UnmarkAsMet();

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> LogTimeAsync(Guid id, LogTimeDto input)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.LogTime(input.Minutes);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> AddWatcherAsync(Guid id, Guid userId)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.AddWatcher(userId);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> RemoveWatcherAsync(Guid id, Guid userId)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.RemoveWatcher(userId);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Create)]
        public async Task<TaskInstanceDto> CreateSubtaskAsync(Guid parentId, CreateTaskInstanceDto input)
        {
            // TODO: Subtask feature not yet implemented in new TaskInstance structure
            // Need to add parent-child relationship support to TaskInstance aggregate
            throw new NotImplementedException("Subtask feature is not yet implemented in the new structure");
        }

        public async Task<List<TaskInstanceDto>> GetSubtasksAsync(Guid parentId)
        {
            // TODO: Subtask feature not yet implemented in new TaskInstance structure
            throw new NotImplementedException("Subtask feature is not yet implemented in the new structure");
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> SetParentAsync(Guid id, SetParentTaskDto input)
        {
            // TODO: Subtask feature not yet implemented in new TaskInstance structure
            throw new NotImplementedException("Subtask feature is not yet implemented in the new structure");
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> RemoveParentAsync(Guid id)
        {
            // TODO: Subtask feature not yet implemented in new TaskInstance structure
            throw new NotImplementedException("Subtask feature is not yet implemented in the new structure");
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> ReorderSubtasksAsync(Guid parentId, ReorderSubtasksDto input)
        {
            // TODO: Subtask feature not yet implemented in new TaskInstance structure
            throw new NotImplementedException("Subtask feature is not yet implemented in the new structure");
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Create)]
        public async Task<TaskInstanceDto> AddDependencyAsync(Guid id, AddDependencyDto input)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);

            // Validate dependent task exists
            var dependentTask = await _taskInstanceRepository.GetAsync(input.DependsOnTaskInstanceId);

            taskInstance.AddDependency(
                input.DependsOnTaskInstanceId,
                input.DependencyType,
                input.LagMinutes,
                input.IsBlocking
            );

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Delete)]
        public async Task<TaskInstanceDto> RemoveDependencyAsync(Guid id, RemoveDependencyDto input)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);

            taskInstance.RemoveDependency(input.DependsOnTaskInstanceId);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        public async Task<List<TaskInstanceDto>> GetDependentTasksAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);

            var dependencyIds = taskInstance.Dependencies
                .Where(d => d.DependsOnTaskInstanceId.HasValue)
                .Select(d => d.DependsOnTaskInstanceId.Value)
                .ToList();

            if (!dependencyIds.Any())
            {
                return new List<TaskInstanceDto>();
            }

            var dependentTasks = await _taskInstanceRepository.GetListAsync(
                new BaseSpecification<TaskInstance>(ti => dependencyIds.Contains(ti.Id))
            );

            return ObjectMapper.Map<List<TaskInstance>, List<TaskInstanceDto>>(dependentTasks);
        }

        public async Task<List<TaskInstanceDto>> GetBlockingTasksAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);

            var blockingDependencyIds = taskInstance.Dependencies
                .Where(d => d.IsBlocking && d.DependsOnTaskInstanceId.HasValue)
                .Select(d => d.DependsOnTaskInstanceId.Value)
                .ToList();

            if (!blockingDependencyIds.Any())
            {
                return new List<TaskInstanceDto>();
            }

            var blockingTasks = await _taskInstanceRepository.GetListAsync(
                new BaseSpecification<TaskInstance>(ti => blockingDependencyIds.Contains(ti.Id))
            );

            return ObjectMapper.Map<List<TaskInstance>, List<TaskInstanceDto>>(blockingTasks);
        }

        public async Task<bool> CanStartAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);

            var dependentInstanceIds = taskInstance.Dependencies
                .Where(d => d.DependsOnTaskInstanceId.HasValue)
                .Select(d => d.DependsOnTaskInstanceId.Value)
                .ToList();

            var dependentInstances = await _taskInstanceRepository.GetListAsync(
                new BaseSpecification<TaskInstance>(ti => dependentInstanceIds.Contains(ti.Id))
            );

            return _dependencyService.CanStartTask(
                taskInstance,
                dependentInstances,
                taskInstance.Dependencies.ToList()
            );
        }

        // TASK MOVEMENT
        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> MoveToProjectAsync(Guid id, MoveTaskDto input)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.MoveToProject(input.NewProjectId);
         
            taskInstance.TransitionTo(input.NewWorkflowStatusId);
            
            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        // FLAGS AND MARKERS
        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> MarkAsNextActionAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            // TODO: MarkAsNextAction needs to update WorkTracking value object
            // For now, this is not yet implemented in the new structure
            throw new NotImplementedException("MarkAsNextAction is not yet implemented in the new structure");

            // await _taskInstanceRepository.UpdateAsync(taskInstance);
            // return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> UnmarkAsNextActionAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            // TODO: UnmarkAsNextAction needs to update WorkTracking value object
            // For now, this is not yet implemented in the new structure
            throw new NotImplementedException("UnmarkAsNextAction is not yet implemented in the new structure");

            // await _taskInstanceRepository.UpdateAsync(taskInstance);
            // return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> FlagTaskAsync(Guid id, string reason)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.Flag(reason);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> UnflagTaskAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.Unflag();

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        // DISCUSSION
        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> LockDiscussionAsync(Guid id)
        {
            // TODO: Discussion locking not yet implemented in new TaskInstance structure
            throw new NotImplementedException("Discussion locking is not yet implemented in the new structure");
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> UnlockDiscussionAsync(Guid id)
        {
            // TODO: Discussion locking not yet implemented in new TaskInstance structure
            throw new NotImplementedException("Discussion locking is not yet implemented in the new structure");
        }

        // BULK OPERATIONS
        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<BulkUpdateResultDto> BulkUpdateAsync(BulkUpdateTaskInstancesDto input)
        {
            var result = new BulkUpdateResultDto();

            foreach (var taskInstanceId in input.TaskInstanceIds)
            {
                try
                {
                    var taskInstance = await _taskInstanceRepository.GetAsync(taskInstanceId);

                    // Update individual properties
                    if (input.AssigneeId.HasValue)
                    {
                        taskInstance.Assign(input.AssigneeId.Value);
                    }

                    if (input.WorkflowStatusId.HasValue)
                    {
                        taskInstance.TransitionTo(input.WorkflowStatusId.Value);
                    }

                    if (input.ProjectId.HasValue)
                    {
                        taskInstance.MoveToProject(input.ProjectId.Value);
                    }

                    if (input.IsFlagged.HasValue)
                    {
                        if (input.IsFlagged.Value)
                        {
                            taskInstance.Flag("Flagged via bulk operation");
                        }
                        else
                        {
                            taskInstance.Unflag();
                        }
                    }

                    // TODO: IsNextAction not yet implemented in new structure
                    // if (input.IsNextAction.HasValue) { ... }

                    await _taskInstanceRepository.UpdateAsync(taskInstance);
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.FailureCount++;
                    result.Errors.Add($"Task {taskInstanceId}: {ex.Message}");
                }
            }

            return result;
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Delete)]
        public async Task<BulkUpdateResultDto> BulkDeleteAsync(Guid[] taskInstanceIds)
        {
            var result = new BulkUpdateResultDto();

            foreach (var taskInstanceId in taskInstanceIds)
            {
                try
                {
                    await _taskInstanceRepository.DeleteAsync(taskInstanceId);
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.FailureCount++;
                    result.Errors.Add($"Task {taskInstanceId}: {ex.Message}");
                }
            }

            return result;
        }

        // STATISTICS & REPORTS
        public async Task<TaskInstanceStatisticsDto> GetStatisticsAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);

            var canStart = await CanStartAsync(id);

            return new TaskInstanceStatisticsDto
            {
                TotalMinutesLogged = taskInstance.WorkTracking.ActualMinutes,
                StepsCompleted = taskInstance.ChecklistItems.Count(sc => sc.IsChecked),
                TotalSteps = taskInstance.ChecklistItems.Count,
                CriteriaMetCount = taskInstance.CriterionCompletions.Count(cc => cc.IsMet),
                TotalCriteria = taskInstance.CriterionCompletions.Count,
                ProgressPercent = taskInstance.WorkTracking.ProgressPercent,
                SubtasksCount = 0, // TODO: Subtasks not yet implemented
                SubtasksDoneCount = 0, // TODO: Subtasks not yet implemented
                DependenciesCount = taskInstance.Dependencies.Count,
                BlockingDependenciesCount = taskInstance.Dependencies.Count(d => d.IsBlocking),
                CanStart = canStart,
                TimeRemaining = taskInstance.Schedule.TimeRemaining
            };
        }

        public async Task<List<TaskInstanceDto>> GetMyTasksAsync()
        {
            if (!CurrentUser.Id.HasValue)
            {
                return new List<TaskInstanceDto>();
            }

            var spec = new BaseSpecification<TaskInstance>(ti =>
                (ti.AssigneeId == CurrentUser.Id.Value || ti.OwnerId == CurrentUser.Id.Value) &&
                !ti.WorkTracking.IsCompleted &&
                !ti.IsDeleted
            );

            var myTasks = await _taskInstanceRepository.GetListAsync(spec);

            return ObjectMapper.Map<List<TaskInstance>, List<TaskInstanceDto>>(
                myTasks.OrderBy(t => t.Schedule.ActualDueDate ?? DateTime.MaxValue).ToList()
            );
        }

        public async Task<List<TaskInstanceDto>> GetDelegatedToMeAsync()
        {
            // TODO: Delegation not yet implemented in new structure
            return new List<TaskInstanceDto>();

            // if (!CurrentUser.Id.HasValue)
            // {
            //     return new List<TaskInstanceDto>();
            // }
            //
            // var spec = new DelegatedTasksSpecification(CurrentUser.Id.Value);
            //
            // var delegatedTasks = await _taskInstanceRepository.GetListAsync(spec);
            //
            // return ObjectMapper.Map<List<TaskInstance>, List<TaskInstanceDto>>(delegatedTasks);
        }

        public async Task<List<TaskInstanceDto>> GetNextActionsAsync(Guid? projectId = null)
        {
            ISpecification<TaskInstance> spec = new NextActionTasksSpecification();

            if (projectId.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti => ti.ProjectId == projectId.Value));
            }

            var nextActions = await _taskInstanceRepository.GetListAsync(spec);

            return ObjectMapper.Map<List<TaskInstance>, List<TaskInstanceDto>>(nextActions);
        }

        // HELPER METHODS
        private async Task<bool> HasCircularParentReferenceAsync(Guid taskId, Guid potentialParentId)
        {
            // TODO: Parent-child relationship not yet implemented in new structure
            // This method is no longer needed as subtasks are not implemented
            return false;
        }
        
        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> AssignToAreaAsync(Guid id, Guid areaId)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.AssignToArea(areaId);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> RemoveFromAreaAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.RemoveFromArea();

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> AssignToContextAsync(Guid id, Guid contextId)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.AssignToContext(contextId);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> RemoveFromContextAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.RemoveFromContext();

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> MoveToAreaAsync(Guid id, Guid areaId)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.MoveToArea(areaId);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        // CHECKLIST ITEMS (Updated from Steps)
        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.UpdateProgress)]
        public async Task<TaskInstanceDto> CheckItemAsync(Guid id, Guid checklistItemId)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);

            taskInstance.CheckItem(checklistItemId, CurrentUser.Id);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.UpdateProgress)]
        public async Task<TaskInstanceDto> UncheckItemAsync(Guid id, Guid checklistItemId)
        {
            var taskInstance = await _taskInstanceRepository.GetWithDetailsAsync(id);

            taskInstance.UncheckItem(checklistItemId);

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        // COLLABORATION
        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> IncrementCommentsAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.IncrementComments();

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.TaskInstances.Update)]
        public async Task<TaskInstanceDto> IncrementAttachmentsAsync(Guid id)
        {
            var taskInstance = await _taskInstanceRepository.GetAsync(id);

            taskInstance.IncrementAttachments();

            await _taskInstanceRepository.UpdateAsync(taskInstance);

            return ObjectMapper.Map<TaskInstance, TaskInstanceDto>(taskInstance);
        }

        // GTD-SPECIFIC QUERIES
        public async Task<List<TaskInstanceDto>> GetTasksByAreaAsync(Guid areaId)
        {
            var spec = new BaseSpecification<TaskInstance>(ti =>
                ti.AreaId == areaId && !ti.IsDeleted);

            var tasks = await _taskInstanceRepository.GetListAsync(spec);

            return ObjectMapper.Map<List<TaskInstance>, List<TaskInstanceDto>>(tasks);
        }

        public async Task<List<TaskInstanceDto>> GetTasksByContextAsync(Guid contextId)
        {
            var spec = new BaseSpecification<TaskInstance>(ti =>
                ti.ContextId == contextId && !ti.IsDeleted);

            var tasks = await _taskInstanceRepository.GetListAsync(spec);

            return ObjectMapper.Map<List<TaskInstance>, List<TaskInstanceDto>>(tasks);
        }

        public async Task<List<TaskInstanceDto>> GetOverdueTasksAsync(Guid? projectId = null, Guid? areaId = null)
        {
            ISpecification<TaskInstance> spec = new BaseSpecification<TaskInstance>(ti =>
                ti.Schedule.ActualDueDate.HasValue &&
                ti.Schedule.ActualDueDate.Value < DateTime.UtcNow &&
                !ti.Schedule.IsCompleted &&
                !ti.IsDeleted
            );

            if (projectId.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti => ti.ProjectId == projectId.Value));
            }

            if (areaId.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti => ti.AreaId == areaId.Value));
            }

            var overdueTasks = await _taskInstanceRepository.GetListAsync(spec);

            return ObjectMapper.Map<List<TaskInstance>, List<TaskInstanceDto>>(overdueTasks);
        }

        public async Task<List<TaskInstanceDto>> GetNextActionsAsync(Guid? projectId = null, Guid? areaId = null,
            Guid? contextId = null)
        {
            ISpecification<TaskInstance> spec = new BaseSpecification<TaskInstance>(ti =>
                ti.WorkTracking.IsNextAction &&
                !ti.WorkTracking.IsCompleted &&
                !ti.IsDeleted
            );

            if (projectId.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti => ti.ProjectId == projectId.Value));
            }

            if (areaId.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti => ti.AreaId == areaId.Value));
            }

            if (contextId.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti => ti.ContextId == contextId.Value));
            }

            var nextActions = await _taskInstanceRepository.GetListAsync(spec);

            return ObjectMapper.Map<List<TaskInstance>, List<TaskInstanceDto>>(nextActions);
        }

        private ISpecification<TaskInstance> CreateListSpecification(GetTaskInstanceListDto input)
        {
            ISpecification<TaskInstance> spec = new BaseSpecification<TaskInstance>(ti => true);

            if (input.ProjectId.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti => ti.ProjectId == input.ProjectId.Value));
            }

            // GTD Filters
            if (input.AreaId.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti => ti.AreaId == input.AreaId.Value));
            }

            if (input.ContextId.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti => ti.ContextId == input.ContextId.Value));
            }

            if (input.AssigneeId.HasValue)
            {
                spec = spec.And(new TaskInstancesByAssigneeSpecification(input.AssigneeId.Value));
            }

            if (input.OwnerId.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti => ti.OwnerId == input.OwnerId.Value));
            }

            if (input.WorkflowStatusId.HasValue)
            {
                spec = spec.And(
                    new BaseSpecification<TaskInstance>(ti => ti.WorkflowStatusId == input.WorkflowStatusId.Value));
            }

            if (input.IsBlocked.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti => ti.IsBlocked == input.IsBlocked.Value));
            }

            if (input.IsNextAction.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti =>
                    ti.WorkTracking.IsNextAction == input.IsNextAction.Value));
            }

            if (input.IsOverdue.HasValue && input.IsOverdue.Value)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti =>
                    ti.Schedule.ActualDueDate.HasValue &&
                    ti.Schedule.ActualDueDate.Value < DateTime.UtcNow &&
                    !ti.Schedule.IsCompleted
                ));
            }

            if (input.Quadrant.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti =>
                    ti.Priority.Quadrant == input.Quadrant.Value));
            }

            if (input.DueDateFrom.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti =>
                    ti.Schedule.ActualDueDate.HasValue &&
                    ti.Schedule.ActualDueDate.Value >= input.DueDateFrom.Value));
            }

            if (input.DueDateTo.HasValue)
            {
                spec = spec.And(new BaseSpecification<TaskInstance>(ti =>
                    ti.Schedule.ActualDueDate.HasValue &&
                    ti.Schedule.ActualDueDate.Value <= input.DueDateTo.Value));
            }

            spec = spec.And(new BaseSpecification<TaskInstance>(ti => !ti.IsDeleted));

            return spec;
        }
    }
}