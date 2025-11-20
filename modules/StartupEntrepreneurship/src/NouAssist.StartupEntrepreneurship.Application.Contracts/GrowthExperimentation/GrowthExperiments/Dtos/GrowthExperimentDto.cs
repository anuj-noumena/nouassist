using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    /// <summary>
    /// DTO representing a growth experiment
    /// </summary>
    public class GrowthExperimentDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Name of the experiment
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the experiment
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Current status of the experiment
        /// </summary>
        public ExperimentStatus Status { get; set; }
        
        /// <summary>
        /// Goal of the experiment
        /// </summary>
        public ExperimentGoalDto Goal { get; set; }
        
        /// <summary>
        /// Target metrics for the experiment
        /// </summary>
        public List<TargetMetricDto> TargetMetrics { get; set; }
        
        /// <summary>
        /// Hypotheses for the experiment
        /// </summary>
        public List<ExperimentHypothesisDto> Hypotheses { get; set; }
        
        /// <summary>
        /// Variants for the experiment
        /// </summary>
        public List<ExperimentVariantDto> Variants { get; set; }
        
        /// <summary>
        /// Start date of the experiment
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// End date of the experiment
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Statistical significance of the experiment results
        /// </summary>
        public StatisticalSignificanceDto Significance { get; set; }
        
        /// <summary>
        /// The winning variant ID, if any
        /// </summary>
        public Guid? WinningVariantId { get; set; }
        
        /// <summary>
        /// Learnings from the experiment
        /// </summary>
        public ExperimentLearningsDto Learnings { get; set; }
    }
}
