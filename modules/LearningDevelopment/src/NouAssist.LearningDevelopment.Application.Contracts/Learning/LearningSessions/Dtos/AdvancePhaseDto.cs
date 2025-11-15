using System.ComponentModel.DataAnnotations;

namespace NouAssist.Learning.LearningSessions.Dtos
{
    public class AdvancePhaseDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int TimeSpentMs { get; set; }
    }
}
