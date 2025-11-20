using System;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts.Dtos
{
    /// <summary>
    /// DTO for filtering and paging elicitation artifact lists
    /// </summary>
    public class GetElicitationArtifactListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public ArtifactType? Type { get; set; }
        public ArtifactStatus? Status { get; set; }
        public Guid? SessionId { get; set; }
        public Guid? CreatedByStakeholderId { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
    }
}
