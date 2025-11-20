using System;
using System.Collections.Generic;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos
{
    /// <summary>
    /// DTO representing prioritization results
    /// </summary>
    public class PrioritizationResultDto
    {
        public Dictionary<Guid, RankScoreDto> RequirementScores { get; set; } = new();
        public decimal OverallConsensus { get; set; }
        public int TotalVotes { get; set; }
        public int ParticipantCount { get; set; }
    }
}
