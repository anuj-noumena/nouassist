using System.Linq;
using NouAssist.Tasks.Aggregates;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.Tasks.Services
{
    /// <summary>
    /// Validates business rules across task entities
    /// </summary>
    public class TaskValidationService : DomainService
    {
        public ValidationResult ValidateTaskForCreation(Aggregates.Task task)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(task.Title))
            {
                result.AddError("Title is required");
            }

            if (task.Title?.Length > TasksConsts.MaxTitleLength)
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

            return result;
        }

        public ValidationResult ValidateTaskInstanceForCompletion(
            TaskInstance instance,
            Aggregates.Task blueprint)
        {
            var result = new ValidationResult();

            if (instance.Schedule.IsCompleted)
            {
                result.AddError("Task is already completed");
            }

            if (instance.BlockingInfo.IsBlocked)
            {
                result.AddError("Cannot complete a blocked task");
            }

            // Check required steps
            var requiredStepIds = blueprint.ActionSteps
                .Where(s => s.IsRequired)
                .Select(s => s.Id)
                .ToList();

            foreach (var requiredStepId in requiredStepIds)
            {
                var completion = instance.StepCompletions
                    .FirstOrDefault(sc => sc.TaskActionStepId == requiredStepId);

                if (completion == null || !completion.IsDone)
                {
                    result.AddError($"Required step {requiredStepId} is not completed");
                }
            }

            return result;
        }

        public bool CanDeleteTask(Aggregates.Task task, int instanceCount)
        {
            // Cannot delete if there are active instances
            return instanceCount == 0 || !task.IsActive;
        }
    }
}