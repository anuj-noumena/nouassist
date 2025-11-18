using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Task = NouAssist.WorkManagement.TaskManagement.Aggregates.Task;

namespace NouAssist.WorkManagement.TaskManagement.Services
{
    /// <summary>
    /// Handles task instance creation from blueprints
    /// </summary>
    public class TaskInstanceFactoryService : DomainService
    {
        public TaskInstance CreateFromBlueprint(
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
                throw new BusinessException("Tasks:InactiveBlueprint");
            }

            // Use blueprint defaults
            var effectiveProjectId = projectId ?? taskBlueprint.ProjectId;
            var effectiveAreaId = areaId ?? taskBlueprint.GetEffectiveAreaForInstance();
            var effectiveContextId = contextId ?? taskBlueprint.GetEffectiveContextForInstance();

            var instance = new TaskInstance(
                GuidGenerator.Create(),
                taskBlueprint.Id,
                workflowId,
                workflowStatusId,
                ownerId,
                effectiveProjectId,
                effectiveAreaId,
                effectiveContextId,
                taskBlueprint.DefaultImportance,
                taskBlueprint.DefaultUrgency,
                taskBlueprint.TenantId,
                assigneeId: assigneeId);

            // Apply due date
            var effectiveDueDate = plannedDueDate ?? taskBlueprint.DefaultDueDate;
            if (effectiveDueDate.HasValue)
            {
                instance.SetPlannedDates(null, effectiveDueDate);
            }

            // Apply estimate
            if (taskBlueprint.DefaultEstimateMinutes.HasValue)
            {
                instance.SetEstimate(taskBlueprint.DefaultEstimateMinutes.Value);
            }

            // Initialize checklist
            var checklistItemIds = taskBlueprint.ChecklistItems.Select(i => i.Id).ToList();
            if (checklistItemIds.Any())
            {
                instance.InitializeChecklist(checklistItemIds);
            }

            return instance;
        }
    }

    /// <summary>
    /// Validates task completion prerequisites
    /// </summary>
    public class TaskCompletionValidator : DomainService
    {
        public ValidationResult ValidateCompletion(
            TaskInstance taskInstance,
            Task taskBlueprint)
        {
            var result = new ValidationResult();

            if (taskInstance.WorkTracking.IsCompleted)
            {
                result.AddError("Task is already completed");
            }

            if (taskInstance.IsBlocked)
            {
                result.AddError("Cannot complete a blocked task");
            }

            // Verify required checklist items
            var requiredItemIds = taskBlueprint.ChecklistItems
                .Where(i => i.IsRequired)
                .Select(i => i.Id)
                .ToList();

            foreach (var requiredId in requiredItemIds)
            {
                var completion = taskInstance.ChecklistItems
                    .FirstOrDefault(ci => ci.ChecklistItemId == requiredId);

                if (completion == null || !completion.IsChecked)
                {
                    result.AddError($"Required checklist item {requiredId} is not completed");
                }
            }

            // Check approval if required
            if (taskInstance.RequiresApproval)
            {
                // This would need to check approval status
                // Simplified here
            }

            return result;
        }
    }

    /// <summary>
    /// Handles recurring task logic
    /// </summary>
    public class TaskRecurrenceProcessor : DomainService
    {
        private readonly TaskInstanceFactoryService _factoryService;

        public TaskRecurrenceProcessor(TaskInstanceFactoryService factoryService)
        {
            _factoryService = factoryService;
        }

        public async Task<List<TaskInstance>> ProcessRecurringTasksAsync(
            DateTime processingDate,
            List<Task> recurringTasks,
            List<TaskInstance> existingInstances)
        {
            var newInstances = new List<TaskInstance>();

            foreach (var task in recurringTasks)
            {
                if (!task.IsRecurring || task.RecurrenceRule == null)
                {
                    continue;
                }

                var shouldCreate = ShouldCreateOccurrence(
                    task,
                    processingDate,
                    existingInstances);

                if (shouldCreate)
                {
                    var nextOccurrence = task.RecurrenceRule.CalculateNextOccurrence(processingDate);

                    // Create instance would require workflow resolution
                    // Simplified here
                }
            }

            return newInstances;
        }

        private bool ShouldCreateOccurrence(
            Task task,
            DateTime checkDate,
            List<TaskInstance> existingInstances)
        {
            // Check if instance already exists for this date
            var existingForDate = existingInstances.Any(ti =>
                ti.TaskId == task.Id &&
                ti.Schedule.PlannedStartDate.HasValue &&
                ti.Schedule.PlannedStartDate.Value.Date == checkDate.Date);

            if (existingForDate)
            {
                return false;
            }

            var nextOccurrence = task.RecurrenceRule.CalculateNextOccurrence(task.CreationTime);
            return checkDate.Date >= nextOccurrence.Date;
        }
    }
}