using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class CreateExperimentGoalDto
    {
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        
        [Range(0.01, 100)]
        public double ExpectedImprovement { get; set; }
        
        [Range(0.01, 100)]
        public double MinimumViableImprovement { get; set; }
    }
}
