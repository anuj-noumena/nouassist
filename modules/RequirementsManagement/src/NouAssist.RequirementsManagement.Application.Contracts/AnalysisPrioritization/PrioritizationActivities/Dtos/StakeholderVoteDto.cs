using System;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos
{
    /// <summary>
    /// DTO representing a stakeholder vote
    /// </summary>
    public class StakeholderVoteDto
    {
        public Guid Id { get; set; }
        public Guid RequirementId { get; set; }
        public Guid StakeholderId { get; set; }
        public VoteValueDto Vote { get; set; } = new();
        public string? Comment { get; set; }
        public DateTime VotedAt { get; set; }
    }
}
