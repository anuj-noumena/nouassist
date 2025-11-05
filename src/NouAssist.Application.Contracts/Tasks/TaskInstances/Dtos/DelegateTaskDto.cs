using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Tasks.TaskInstances.Dtos
{
    public class DelegateTaskDto : IValidatableObject
    {
        [Required]
        public Guid DelegatedTo { get; set; }

        public DateTime? DueBy { get; set; }

        public bool FollowupRequired { get; set; } = true;

        public DateTime? FollowupAt { get; set; }

        [StringLength(2000)]
        public string Note { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FollowupRequired && !FollowupAt.HasValue)
            {
                yield return new ValidationResult(
                    "Follow-up date is required when follow-up is required.",
                    new[] { nameof(FollowupAt) }
                );
            }

            if (DueBy.HasValue && DueBy.Value < DateTime.UtcNow)
            {
                yield return new ValidationResult(
                    "Due date cannot be in the past.",
                    new[] { nameof(DueBy) }
                );
            }
        }
    }

    public class AcceptDelegationDto
    {
        [StringLength(2000)]
        public string ResponseNote { get; set; }
    }

    public class DeclineDelegationDto
    {
        [Required]
        [StringLength(2000)]
        public string ResponseNote { get; set; }
    }
}
