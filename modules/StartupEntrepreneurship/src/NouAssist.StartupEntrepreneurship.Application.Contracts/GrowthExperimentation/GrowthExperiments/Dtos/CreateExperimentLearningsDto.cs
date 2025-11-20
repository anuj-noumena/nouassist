using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class CreateExperimentLearningsDto
    {
        [Required]
        [StringLength(GrowthExperimentationConstants.Experiments.MaxLearningsTextLength)]
        public string Insights { get; set; }
        
        [StringLength(GrowthExperimentationConstants.Experiments.MaxLearningsTextLength)]
        public string UnexpectedOutcomes { get; set; }
        
        [StringLength(GrowthExperimentationConstants.Experiments.MaxLearningsTextLength)]
        public string Recommendations { get; set; }
    }
}
