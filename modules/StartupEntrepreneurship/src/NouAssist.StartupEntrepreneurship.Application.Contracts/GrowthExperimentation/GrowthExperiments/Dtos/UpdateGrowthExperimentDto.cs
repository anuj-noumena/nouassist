using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class UpdateGrowthExperimentDto
    {
        /// <summary>
        /// Name of the experiment
        /// </summary>
        [Required]
        [StringLength(GrowthExperimentationConstants.Experiments.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the experiment
        /// </summary>
        [StringLength(GrowthExperimentationConstants.Experiments.MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
