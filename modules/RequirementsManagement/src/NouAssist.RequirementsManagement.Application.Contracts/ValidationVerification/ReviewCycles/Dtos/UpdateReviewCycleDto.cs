using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.ValidationVerification.Constants;

namespace NouAssist.RequirementsManagement.ValidationVerification.ReviewCycles.Dtos
{
    /// <summary>
    /// Input DTO for updating a review cycle
    /// </summary>
    public class UpdateReviewCycleDto
    {
        [Required]
        [StringLength(ValidationVerificationConsts.RequirementReviewCycle.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(ValidationVerificationConsts.RequirementReviewCycle.MaxDescriptionLength)]
        public string Description { get; set; }

        public DateTime? TargetCompletionDate { get; set; }
    }
}
