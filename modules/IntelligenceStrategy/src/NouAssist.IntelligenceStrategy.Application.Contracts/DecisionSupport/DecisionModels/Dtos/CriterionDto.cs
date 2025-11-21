using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos
{
    /// <summary>
    /// DTO for displaying criterion details
    /// </summary>
    public class CriterionDto : EntityDto<Guid>
    {
        /// <summary>
        /// The decision model this criterion belongs to
        /// </summary>
        public Guid DecisionModelId { get; set; }
        
        /// <summary>
        /// Name of the criterion
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the criterion
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Weight of this criterion in the decision (normalized between 0 and 1)
        /// </summary>
        public decimal Weight { get; set; }
    }
}
