using System;
using NouAssist.RequirementsManagement.ChangeManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// DTO for approval step
    /// </summary>
    public class ApprovalStepDto : AuditedEntityDto<Guid>
    {
        public string ApproverRoleName { get; set; }
        public int ApproverRolePriority { get; set; }
        public Guid ApproverId { get; set; }
        public string ApproverName { get; set; }
        public int SequenceOrder { get; set; }
        public ApprovalDecision Decision { get; set; }
        public string DecisionString => Decision.ToString();
        public string DecisionReason { get; set; }
        public DateTime? DecisionDate { get; set; }
        public bool IsCompleted => Decision != ApprovalDecision.Pending;
    }
}
