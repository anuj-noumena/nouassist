using System;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationSessions.Dtos
{
    /// <summary>
    /// DTO for filtering and paging elicitation session lists
    /// </summary>
    public class GetElicitationSessionListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public SessionType? Type { get; set; }
        public SessionStatus? Status { get; set; }
        public Guid? OrganizerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IncludeParticipants { get; set; }
        public bool? IncludeArtifacts { get; set; }
    }
}
