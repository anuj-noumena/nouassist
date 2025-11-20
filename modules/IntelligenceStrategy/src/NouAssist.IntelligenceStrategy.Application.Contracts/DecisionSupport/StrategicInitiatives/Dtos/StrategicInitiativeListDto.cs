using System;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives.Dtos
{
    /// <summary>
    /// DTO for simplified list view of strategic initiatives
    /// </summary>
    public class StrategicInitiativeListDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Title of the strategic initiative
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Current status of the proposal
        /// </summary>
        public ProposalStatus Status { get; set; }
        
        /// <summary>
        /// String representation of the status
        /// </summary>
        public string StatusName { get; set; }
        
        /// <summary>
        /// The degree to which this initiative aligns with strategic goals
        /// </summary>
        public AlignmentStatus StrategicAlignment { get; set; }
        
        /// <summary>
        /// String representation of the alignment status
        /// </summary>
        public string AlignmentName { get; set; }
        
        /// <summary>
        /// When the initiative was proposed
        /// </summary>
        public DateTime ProposedDate { get; set; }
        
        /// <summary>
        /// When the initiative was approved or rejected (if applicable)
        /// </summary>
        public DateTime? DecisionDate { get; set; }
        
        /// <summary>
        /// If approved, reference to the project created in the Project BC
        /// </summary>
        public Guid? ProjectId { get; set; }
    }
}
