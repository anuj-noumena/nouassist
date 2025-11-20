using System;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos
{
    /// <summary>
    /// DTO for filtering prioritization activities list
    /// </summary>
    public class GetPrioritizationActivityListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public ActivityStatus? Status { get; set; }
        public PrioritizationMethod? Method { get; set; }
        public Guid? ParticipantId { get; set; }
        public Guid? RequirementId { get; set; }
        public bool? ActiveOnly { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
    }
}
