using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using NouAssist.RequirementsManagement.ChangeManagement.Enums;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// DTO for returning change request details
    /// </summary>
    public class ChangeRequestDto : FullAuditedEntityDto<Guid>
    {
        public string RequestIdentifier { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public AffectedRequirementDto AffectedRequirement { get; set; }
        public RequestStatus Status { get; set; }
        public string StatusString => Status.ToString();
        public Guid SubmitterId { get; set; }
        public string SubmitterName { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        
        public List<ChangeRationaleDto> Rationales { get; set; } = new();
        public List<ChangeImpactStatementDto> ImpactStatements { get; set; } = new();
        public List<ApprovalStepDto> ApprovalSteps { get; set; } = new();
        
        public decimal? TotalCostImpact { get; set; }
        public int? TotalTimeImpact { get; set; }
        public bool CanEdit { get; set; }
    }
}
