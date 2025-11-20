using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class CreateHypothesisDto
    {
        [Required]
        [StringLength(GrowthExperimentationConstants.Experiments.MaxHypothesisTextLength)]
        public string Statement { get; set; }
        
        [StringLength(GrowthExperimentationConstants.Experiments.MaxDescriptionLength)]
        public string Rationale { get; set; }
    }
}
