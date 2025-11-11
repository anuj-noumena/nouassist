using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
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
        private readonly ITaskCompletionPolicy _taskCompleteionPolicy;

        public TaskInstanceProcessingService(ITaskCompletionPolicy taskCompleteionPolicy,
            ILogger<TaskInstanceProcessingService> logger)
        {
            _taskCompleteionPolicy = taskCompleteionPolicy;
            _logger = logger;
        }

        public TaskInstanceProcessingResult ProcessTaskCompletion(
            TaskInstance taskInstance,
            Task taskBlueprint,
            Guid completedBy,
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

                // Complete the task instance
                taskInstance.Complete(completedBy, _taskCompleteionPolicy, resolution, resolutionNote);

                result.ProcessedTaskInstance = taskInstance;
                result.IsSuccessful = true;

                _logger.LogInformation(
                    "Task instance {TaskInstanceId} completed successfully by user {UserId}",
                    taskInstance.Id, completedBy);
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
            Guid projectId,
            Guid workflowStatusId,
            Guid ownerId,
            Guid? assigneeId = null,
            DateTime? plannedDueDate = null)
        {
            if (!taskBlueprint.IsActive)
            {
                throw new BusinessException("Tasks:CannotCreateInstanceFromInactiveTask");
            }

            // Create instance with default values from blueprint
            var instance = new TaskInstance(
                Guid.NewGuid(),
                taskBlueprint.Id,
                projectId,
                workflowStatusId,
                ownerId,
                taskBlueprint.DefaultImportance,
                taskBlueprint.DefaultUrgency,
                taskBlueprint.TenantId,
                assigneeId);

            // Apply default due date from blueprint if not provided
            instance.Schedule = instance.Schedule.WithPlannedDates(due: plannedDueDate ?? taskBlueprint.DefaultDueDate);
            // Initialize step completions from blueprint action steps
            var actionStepIds = taskBlueprint.ActionSteps.Select(s => s.Id).ToList();
            instance.InitializeStepCompletions(actionStepIds);

            // Initialize criterion completions from blueprint acceptance criteria
            var criterionIds = taskBlueprint.AcceptanceCriteria.Select(c => c.Id).ToList();
            instance.InitializeCriterionCompletions(criterionIds);

            _logger.LogInformation(
                "Created task instance {InstanceId} from blueprint {TaskId}",
                instance.Id, taskBlueprint.Id);

            return instance;
        }

        public void ProcessRecurringTaskOccurrence(
            Task taskBlueprint,
            Guid projectId,
            Guid workflowStatusId,
            DateTime occurrenceDate)
        {
            if (!taskBlueprint.IsRecurring)
            {
                throw new BusinessException("Tasks:TaskIsNotRecurring");
            }

            if (taskBlueprint.RecurrenceRule == null)
            {
                throw new BusinessException("Tasks:MissingRecurrenceRule");
            }

            // Calculate next occurrence
            var nextOccurrence = taskBlueprint.RecurrenceRule.CalculateNextOccurrence(occurrenceDate);

            _logger.LogInformation(
                "Processing recurring task {TaskId}, next occurrence: {NextOccurrence}",
                taskBlueprint.Id, nextOccurrence);

            // This service doesn't create the instance directly - it delegates to the application layer
            // But it can validate and calculate the next occurrence date
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

            if (taskInstance.BlockingInfo.IsBlocked)
            {
                result.AddError("Cannot complete a blocked task");
            }

            // Verify all required action steps are completed
            var requiredStepIds = taskBlueprint.ActionSteps
                .Where(s => s.IsRequired)
                .Select(s => s.Id)
                .ToList();

            foreach (var requiredStepId in requiredStepIds)
            {
                var completion = taskInstance.StepCompletions
                    .FirstOrDefault(sc => sc.TaskActionStepId == requiredStepId);

                if (completion == null || !completion.IsDone)
                {
                    result.AddError($"Required step {requiredStepId} is not completed");
                }
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
        public List<string> Errors { get; set; } = new();

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}