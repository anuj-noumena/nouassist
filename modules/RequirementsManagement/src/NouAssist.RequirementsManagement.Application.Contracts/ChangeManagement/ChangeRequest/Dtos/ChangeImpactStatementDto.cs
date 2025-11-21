using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// DTO for change impact statement
    /// </summary>
    public class ChangeImpactStatementDto : AuditedEntityDto<Guid>
    {
        public ImpactDetailsDto ImpactDetails { get; set; }
        public string Description { get; set; }
        public DateTime AssessmentDate { get; set; }
    }
}
