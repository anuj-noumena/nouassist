using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos
{
    /// <summary>
    /// DTO representing a prioritization activity
    /// </summary>
    public class PrioritizationActivityDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PrioritizationMethod Method { get; set; }
        public ActivityStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool AllowModification { get; set; }
        public string? Instructions { get; set; }
        public List<Guid> ParticipantIds { get; set; } = new();
        public List<Guid> RequirementIds { get; set; } = new();
        public List<StakeholderVoteDto> Votes { get; set; } = new();
        public decimal VotingProgress { get; set; }
    }
}
