using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using NouAssist.WorkManagement.TaskManagement.Policies;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.TaskManagement.Services
{
    /// <summary>
    /// Coordinates complex operations across Task and TaskInstance aggregates
    /// </summary>
    public class TaskInstanceProcessingService : DomainService
    {
        private readonly ILogger<TaskInstanceProcessingService> _logger;
        private readonly ITaskCompletionPolicy _taskCompletionPolicy;

        public TaskInstanceProcessingService(
            ITaskCompletionPolicy taskCompletionPolicy,
            ILogger<TaskInstanceProcessingService> logger)
        {
            _taskCompletionPolicy = taskCompletionPolicy;
            _logger = logger;
        }

        public TaskInstanceProcessingResult ProcessTaskCompletion(
            TaskInstance taskInstance,
            Task taskBlueprint,
            string resolution = null,
            string resolutionNote = null)
        {
            var result = new TaskInstanceProcessingResult();

            try
            {
                // Validate completion prerequisites
                var validation = ValidateCompletionPrerequisites(taskInstance, taskBlueprint);
                if (!validation.IsValid)
                {
                    result.Errors = validation.Errors;
                    return result;
                }

                // Apply completion policy if needed
                if (_taskCompletionPolicy != null)
                {
                    PolicyResult policyResult = _taskCompletionPolicy.CanComplete(taskInstance);
                    if (!policyResult.IsAllowed)
                    {
                        result.Errors.Add(policyResult.Reason);
                        return result;
                    }
                }

                // Complete the task instance
                taskInstance.Complete(resolution ?? "Completed", resolutionNote);

                result.ProcessedTaskInstance = taskInstance;
                result.IsSuccessful = true;

                _logger.LogInformation(
                    "Task instance {TaskInstanceId} completed successfully",
                    taskInstance.Id);
            }
            catch (BusinessException ex)
            {
                result.Errors.Add(ex.Message);
                _logger.LogWarning(ex, "Failed to process task completion");
            }

            return result;
        }

        public TaskInstance CreateTaskInstanceFromBlueprint(
            Task taskBlueprint,
            Guid workflowId,
            Guid workflowStatusId,
            Guid ownerId,
            Guid? projectId = null,
            Guid? areaId = null,
            Guid? contextId = null,
            Guid? assigneeId = null,
            DateTime? plannedDueDate = null)
        {
            if (!taskBlueprint.IsActive)
            {
                throw new BusinessException("Tasks:CannotCreateInstanceFromInactiveTask");
            }

            // Determine effective area and context from blueprint if not provided
            var effectiveAreaId = areaId ?? taskBlueprint.GetEffectiveAreaForInstance();
            var effectiveContextId = contextId ?? taskBlueprint.GetEffectiveContextForInstance();

            // Validate: cannot be in both project and area
            if (projectId.HasValue && effectiveAreaId.HasValue)
            {
                throw new BusinessException("Tasks:CannotBeInBothProjectAndArea");
            }

            // Create instance with default values from blueprint
            var instance = new TaskInstance(
                Guid.NewGuid(),
                taskBlueprint.Id,
                workflowId,
                workflowStatusId,
                ownerId,
                projectId,
                effectiveAreaId,
                effectiveContextId,
                taskBlueprint.DefaultImportance,
                taskBlueprint.DefaultUrgency,
                tenantId: taskBlueprint.TenantId,
                assigneeId: assigneeId);

            // Apply default due date from blueprint if not provided
            var dueDate = plannedDueDate ?? taskBlueprint.DefaultDueDate;
            if (dueDate.HasValue)
            {
                instance.SetPlannedDates(null, dueDate);
            }

            // Apply default estimate if available
            if (taskBlueprint.DefaultEstimateMinutes.HasValue)
            {
                instance.SetEstimate(taskBlueprint.DefaultEstimateMinutes.Value);
            }

            // Initialize checklist from blueprint
            if (taskBlueprint.HasChecklist)
            {
                var checklistItemIds = taskBlueprint.ChecklistItems.Select(s => s.Id).ToList();
                instance.InitializeChecklist(checklistItemIds);
            }

            // Note: Acceptance criteria completions are tracked separately
            // They can be managed through a separate aggregate or service if needed

            _logger.LogInformation(
                "Created task instance {InstanceId} from blueprint {TaskId}",
                instance.Id, taskBlueprint.Id);

            return instance;
        }

        public DateTime? CalculateNextRecurrenceDate(Task taskBlueprint, DateTime currentDate)
        {
            if (!taskBlueprint.IsRecurring)
            {
                return null;
            }

            if (taskBlueprint.RecurrenceRule == null)
            {
                throw new BusinessException("Tasks:MissingRecurrenceRule");
            }

            // Calculate next occurrence
            var nextOccurrence = taskBlueprint.RecurrenceRule.CalculateNextOccurrence(currentDate);

            _logger.LogInformation(
                "Calculated next recurrence for task {TaskId}: {NextOccurrence}",
                taskBlueprint.Id, nextOccurrence);

            return nextOccurrence;
        }

        private ValidationResult ValidateCompletionPrerequisites(
            TaskInstance taskInstance,
            Task taskBlueprint)
        {
            var result = new ValidationResult();

            if (taskInstance.Schedule.IsCompleted)
            {
                result.AddError("Task is already completed");
            }

            if (taskInstance.IsBlocked)
            {
                result.AddError("Cannot complete a blocked task");
            }

            // Check if task has blocking dependencies
            if (taskInstance.HasBlockingDependencies)
            {
                result.AddError("Task has blocking dependencies that are not completed");
            }

            // Verify all required checklist items are completed
            if (taskBlueprint.HasChecklist)
            {
                var requiredItemIds = taskBlueprint.ChecklistItems
                    .Where(s => s.IsRequired)
                    .Select(s => s.Id)
                    .ToList();

                foreach (var requiredItemId in requiredItemIds)
                {
                    var completion = taskInstance.ChecklistItems
                        .FirstOrDefault(sc => sc.ChecklistItemId == requiredItemId);

                    if (completion == null || !completion.IsChecked)
                    {
                        var itemTitle = taskBlueprint.ChecklistItems
                            .FirstOrDefault(ci => ci.Id == requiredItemId)?.Title ?? "Unknown";
                        result.AddError($"Required checklist item '{itemTitle}' is not completed");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Validates if a task can be started
        /// </summary>
        public ValidationResult ValidateStartPrerequisites(TaskInstance taskInstance)
        {
            var result = new ValidationResult();

            if (taskInstance.WorkTracking.HasStarted)
            {
                result.AddError("Task has already been started");
            }

            if (taskInstance.IsBlocked)
            {
                result.AddError("Cannot start a blocked task");
            }

            if (taskInstance.HasBlockingDependencies)
            {
                result.AddError("Task has blocking dependencies that must be completed first");
            }

            return result;
        }
    }

    public class TaskInstanceProcessingResult
    {
        public bool IsSuccessful { get; set; }
        public TaskInstance ProcessedTaskInstance { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public System.Collections.Generic.List<string> Errors { get; set; } = new();

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}