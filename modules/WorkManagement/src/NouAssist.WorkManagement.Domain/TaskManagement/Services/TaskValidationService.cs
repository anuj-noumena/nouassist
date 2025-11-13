using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.TaskManagement.Services
{
    /// <summary>
    /// Validates business rules across task entities
    /// </summary>
    public class TaskValidationService : DomainService
    {
        public ValidationResult ValidateTaskForCreation(Task task)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(task.Title))
            {
                result.AddError("Title is required");
            }

            if (task.Title.Length > TasksConsts.MaxTitleLength)
            {
                result.AddError($"Title cannot exceed {TasksConsts.MaxTitleLength} characters");
            }

            if (task.DefaultImportance < 0 || task.DefaultImportance > 1)
            {
                result.AddError("Importance must be between 0 and 1");
            }

            if (task.DefaultUrgency < 0 || task.DefaultUrgency > 1)
            {
                result.AddError("Urgency must be between 0 and 1");
            }

            // Validate GTD organization - cannot be in both project and area
            if (task.ProjectId.HasValue && task.AreaId.HasValue)
            {
                result.AddError("Task cannot belong to both a Project and an Area");
            }

            return result;
        }

        public ValidationResult ValidateTaskInstanceForCompletion(
            TaskInstance instance,
            Task blueprint)
        {
            var result = new ValidationResult();

            if (instance.Schedule.IsCompleted)
            {
                result.AddError("Task is already completed");
            }

            if (instance.IsBlocked)
            {
                result.AddError("Cannot complete a blocked task");
            }

            if (instance.HasBlockingDependencies)
            {
                result.AddError("Task has blocking dependencies that must be resolved first");
            }

            // Check required checklist items
            if (blueprint.HasChecklist)
            {
                var requiredItemIds = blueprint.ChecklistItems
                    .Where(s => s.IsRequired)
                    .Select(s => s.Id)
                    .ToList();

                foreach (var requiredItemId in requiredItemIds)
                {
                    var completion = instance.ChecklistItems
                        .FirstOrDefault(sc => sc.ChecklistItemId == requiredItemId);

                    if (completion == null || !completion.IsChecked)
                    {
                        var itemTitle = blueprint.ChecklistItems
                            .FirstOrDefault(ci => ci.Id == requiredItemId)?.Title ?? "Unknown";
                        result.AddError($"Required checklist item '{itemTitle}' is not completed");
                    }
                }
            }

            // Check acceptance criteria if needed
            if (blueprint.HasAcceptanceCriteria)
            {
                result.AddError("Acceptance criteria validation not yet implemented");
            }

            return result;
        }

        public ValidationResult ValidateTaskInstanceForStart(TaskInstance instance)
        {
            var result = new ValidationResult();

            if (instance.WorkTracking.HasStarted)
            {
                result.AddError("Task has already been started");
            }

            if (instance.IsBlocked)
            {
                result.AddError("Cannot start a blocked task");
            }

            if (instance.HasBlockingDependencies)
            {
                result.AddError("Task has blocking dependencies that must be completed first");
            }

            return result;
        }

        public ValidationResult ValidateTaskInstanceAssignment(
            TaskInstance instance,
            System.Guid newAssigneeId)
        {
            var result = new ValidationResult();

            if (instance.Schedule.IsCompleted)
            {
                result.AddError("Cannot reassign a completed task");
            }

            if (instance.OwnerId == newAssigneeId)
            {
                // Assigning to owner is allowed but may want to warn
            }

            return result;
        }

        public bool CanDeleteTask(Task task, int instanceCount)
        {
            // Cannot delete if there are active instances
            return instanceCount == 0 || !task.IsActive;
        }

        public ValidationResult ValidateTaskOrganization(Task task)
        {
            var result = new ValidationResult();

            // Validate: cannot be in both project and area
            if (task.ProjectId.HasValue && task.AreaId.HasValue)
            {
                result.AddError("Task cannot belong to both a Project and an Area");
            }

            return result;
        }

        public ValidationResult ValidateTaskInstanceOrganization(TaskInstance instance)
        {
            var result = new ValidationResult();

            // Validate: cannot be in both project and area
            if (instance.ProjectId.HasValue && instance.AreaId.HasValue)
            {
                result.AddError("Task instance cannot belong to both a Project and an Area");
            }

            return result;
        }
    }
}