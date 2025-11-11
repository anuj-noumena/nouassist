using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.TaskManagement.Enums;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos
{
    public class UpdateTaskStatusDto : IValidatableObject
    {
        [Required]
        public WorkflowStatusCategory NewStatus { get; set; }

        [StringLength(200)]
        public string Resolution { get; set; }

        [StringLength(2000)]
        public string ResolutionNote { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewStatus == WorkflowStatusCategory.Cancelled && 
                string.IsNullOrWhiteSpace(ResolutionNote))
            {
                yield return new ValidationResult(
                    "A cancellation reason is required when cancelling a task.",
                    new[] { nameof(ResolutionNote) }
                );
            }

            if (NewStatus == WorkflowStatusCategory.Done &&
                string.IsNullOrWhiteSpace(Resolution))
            {
                yield return new ValidationResult(
                    "A resolution is required when completing a task.",
                    new[] { nameof(Resolution) }
                );
            }
        }
    }

    public class BlockTaskDto
    {
        [Required]
        [StringLength(1000)]
        public string Reason { get; set; }
    }

    public class CompleteStepDto
    {
        [Required]
        public Guid StepId { get; set; }
    }

    public class MarkCriterionMetDto
    {
        [Required]
        public Guid CriterionId { get; set; }
    }

    public class LogTimeDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Minutes { get; set; }

        [StringLength(500)]
        public string Note { get; set; }
    }
}
