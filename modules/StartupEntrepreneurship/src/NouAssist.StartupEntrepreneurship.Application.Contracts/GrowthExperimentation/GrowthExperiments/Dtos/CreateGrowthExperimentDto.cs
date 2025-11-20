using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class CreateGrowthExperimentDto
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
        
        /// <summary>
        /// Goal of the experiment
        /// </summary>
        [Required]
        public CreateExperimentGoalDto Goal { get; set; }
        
        /// <summary>
        /// Primary target metric
        /// </summary>
        [Required]
        public CreateTargetMetricDto PrimaryMetric { get; set; }
        
        /// <summary>
        /// Secondary target metrics
        /// </summary>
        public List<CreateTargetMetricDto> SecondaryMetrics { get; set; }
    }
}
