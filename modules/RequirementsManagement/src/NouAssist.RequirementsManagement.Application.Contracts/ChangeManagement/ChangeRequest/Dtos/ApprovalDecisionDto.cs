using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;
using NouAssist.RequirementsManagement.ChangeManagement.Enums;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// Input DTO for submitting an approval decision
    /// </summary>
    public class ApprovalDecisionDto
    {
        [Required]
        public Guid ApprovalStepId { get; set; }

        [Required]
        public ApprovalDecision Decision { get; set; }

        [Required]
        [StringLength(ChangeManagementConsts.MaxDecisionReasonLength)]
        public string DecisionReason { get; set; }
    }
}
