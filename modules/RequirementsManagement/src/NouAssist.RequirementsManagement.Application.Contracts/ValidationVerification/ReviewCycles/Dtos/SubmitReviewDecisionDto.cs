using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.ValidationVerification.Constants;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;

namespace NouAssist.RequirementsManagement.ValidationVerification.ReviewCycles.Dtos
{
    /// <summary>
    /// Input DTO for submitting a review decision
    /// </summary>
    public class SubmitReviewDecisionDto
    {
        [Required]
        public ReviewDecisionType DecisionType { get; set; }

        [StringLength(ValidationVerificationConsts.ReviewFeedback.MaxReasonLength)]
        public string Reason { get; set; }

        [StringLength(ValidationVerificationConsts.ReviewFeedback.MaxCommentLength)]
        public string Comment { get; set; }
    }
}
