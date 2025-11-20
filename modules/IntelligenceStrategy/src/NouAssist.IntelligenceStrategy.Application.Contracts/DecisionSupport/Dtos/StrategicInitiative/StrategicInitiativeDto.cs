using System;
using NouAssist.DecisionSupport.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.DecisionSupport.Dtos.StrategicInitiative
{
    /// <summary>
    /// DTO for strategic initiative data
    /// </summary>
    public class StrategicInitiativeDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// The title of the initiative
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// A detailed description of the initiative
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The expected outcomes or goals of the initiative
        /// </summary>
        public string ExpectedOutcomes { get; set; }
        
        /// <summary>
        /// The rationale or business case for the initiative
        /// </summary>
        public string Rationale { get; set; }
        
        /// <summary>
        /// ID of the decision model that led to this initiative, if any
        /// </summary>
        public Guid? DecisionModelId { get; set; }
        
        /// <summary>
        /// Current status of the proposal
        /// </summary>
        public ProposalStatus Status { get; set; }
        
        /// <summary>
        /// Reason for rejection, if applicable
        /// </summary>
        public string RejectionReason { get; set; }
        
        /// <summary>
        /// ID of the project created from this initiative, if approved
        /// </summary>
        public Guid? ProjectId { get; set; }
        
        /// <summary>
        /// Date when the proposal was approved or rejected
        /// </summary>
        public DateTime? DecisionDate { get; set; }
    }
}
