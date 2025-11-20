using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos
{
    /// <summary>
    /// DTO for submitting a vote
    /// </summary>
    public class SubmitVoteDto
    {
        [Required]
        public Guid RequirementId { get; set; }

        [Required]
        public VoteValueDto Vote { get; set; } = new();

        [StringLength(1000)]
        public string? Comment { get; set; }
    }
}
