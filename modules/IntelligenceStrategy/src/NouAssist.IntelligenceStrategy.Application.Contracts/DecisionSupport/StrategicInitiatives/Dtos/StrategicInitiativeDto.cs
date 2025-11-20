using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives.Dtos
{
    /// <summary>
    /// DTO for displaying strategic initiative details
    /// </summary>
    public class StrategicInitiativeDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Title of the strategic initiative
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Detailed description of the initiative
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The rationale behind this strategic initiative
        /// </summary>
        public string Rationale { get; set; }
        
        /// <summary>
        /// Expected outcomes from implementing this initiative
        /// </summary>
        public string ExpectedOutcomes { get; set; }
        
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
        /// If approved, reference to the project created in the Project BC
        /// </summary>
        public Guid? ProjectId { get; set; }
        
        /// <summary>
        /// For rejected initiatives, contains the reason for rejection
        /// </summary>
        public string RejectionReason { get; set; }
        
        /// <summary>
        /// ID of any related decision model that informed this initiative
        /// </summary>
        public Guid? RelatedDecisionModelId { get; set; }
        
        /// <summary>
        /// When the initiative was proposed
        /// </summary>
        public DateTime ProposedDate { get; set; }
        
        /// <summary>
        /// When the initiative was approved or rejected (if applicable)
        /// </summary>
        public DateTime? DecisionDate { get; set; }
        
        /// <summary>
        /// List of supporting analysis references related to this initiative
        /// </summary>
        public List<AnalysisReferenceDto> SupportingAnalyses { get; set; }
    }
}
