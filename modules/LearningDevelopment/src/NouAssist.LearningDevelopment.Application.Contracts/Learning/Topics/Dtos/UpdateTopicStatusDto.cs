using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.Learning.Enums;

namespace NouAssist.LearningDevelopment.Learning.Topics.Dtos
{
    public class UpdateTopicStatusDto : IValidatableObject
    {
        [Required]
        public TopicStatus Status { get; set; }

        public string Reason { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Require reason when archiving
            if (Status == TopicStatus.Archived && string.IsNullOrWhiteSpace(Reason))
            {
                yield return new ValidationResult(
                    "A reason is required when archiving a topic.",
                    new[] { nameof(Reason) }
                );
            }

            // Cannot transition directly from Planned to Completed
            // Note: In production, you'd need access to current status
            // This is a simplified example
        }
    }
}
