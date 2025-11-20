using System;
using System.Collections.Generic;
using NouAssist.DecisionSupport.Dtos.Criterion;
using NouAssist.DecisionSupport.Dtos.Alternative;
using Volo.Abp.Application.Dtos;

namespace NouAssist.DecisionSupport.Dtos.DecisionModel
{
    /// <summary>
    /// DTO for detailed decision model data including criteria and alternatives
    /// </summary>
    public class DecisionModelDetailDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// The name of the decision model
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// A description of the decision to be made
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The context or background information for the decision
        /// </summary>
        public string Context { get; set; }
        
        /// <summary>
        /// Date when the decision needs to be made
        /// </summary>
        public DateTime? DecisionDueDate { get; set; }
        
        /// <summary>
        /// Criteria used in this decision model
        /// </summary>
        public List<CriterionDto> Criteria { get; set; }
        
        /// <summary>
        /// Alternatives being considered in this decision model
        /// </summary>
        public List<AlternativeDto> Alternatives { get; set; }
        
        public DecisionModelDetailDto()
        {
            Criteria = new List<CriterionDto>();
            Alternatives = new List<AlternativeDto>();
        }
    }
}
