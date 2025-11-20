using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.DecisionSupport.Dtos.DecisionModel
{
    /// <summary>
    /// DTO for decision model data
    /// </summary>
    public class DecisionModelDto : AuditedEntityDto<Guid>
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
        /// Number of criteria defined in this model
        /// </summary>
        public int CriteriaCount { get; set; }
        
        /// <summary>
        /// Number of alternatives defined in this model
        /// </summary>
        public int AlternativesCount { get; set; }
    }
}
