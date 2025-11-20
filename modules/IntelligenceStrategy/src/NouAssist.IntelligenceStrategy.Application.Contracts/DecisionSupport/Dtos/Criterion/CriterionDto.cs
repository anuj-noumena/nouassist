using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.DecisionSupport.Dtos.Criterion
{
    /// <summary>
    /// DTO for criterion data
    /// </summary>
    public class CriterionDto : EntityDto<Guid>
    {
        /// <summary>
        /// The decision model this criterion belongs to
        /// </summary>
        public Guid DecisionModelId { get; set; }
        
        /// <summary>
        /// The name of the criterion
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the criterion
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The weight of the criterion (0-1)
        /// </summary>
        public decimal Weight { get; set; }
    }
}
