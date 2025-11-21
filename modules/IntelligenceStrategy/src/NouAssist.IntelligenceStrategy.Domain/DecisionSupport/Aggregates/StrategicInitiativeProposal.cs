using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using NouAssist.IntelligenceStrategy.DecisionSupport.Entities;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using NouAssist.IntelligenceStrategy.DecisionSupport.Events;
using NouAssist.IntelligenceStrategy.DecisionSupport.Exceptions;
using NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates
{
    /// <summary>
    /// Represents a proposed initiative derived from strategic goals or decisions.
    /// If approved, it typically triggers project creation in the Project BC.
    /// </summary>
    public class StrategicInitiativeProposal : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        #region Properties
        
        /// <summary>
        /// The tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Title of the strategic initiative
        /// </summary>
        public string Title { get; private set; }
        
        /// <summary>
        /// Detailed description of the initiative
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// The rationale behind this strategic initiative
        /// </summary>
        public DecisionRationaleText Rationale { get; private set; }
        
        /// <summary>
        /// Expected outcomes from implementing this initiative
        /// </summary>
        public ExpectedOutcomeText ExpectedOutcomes { get; private set; }
        
        /// <summary>
        /// Current status of the proposal
        /// </summary>
        public ProposalStatus Status { get; private set; }
        
        /// <summary>
        /// The degree to which this initiative aligns with strategic goals
        /// </summary>
        public AlignmentStatus StrategicAlignment { get; private set; }
        
        /// <summary>
        /// If approved, reference to the project created in the Project BC
        /// </summary>
        public ProjectId? ProjectId { get; private set; }
        
        /// <summary>
        /// For rejected initiatives, contains the reason for rejection
        /// </summary>
        public string RejectionReason { get; private set; }
        
        /// <summary>
        /// ID of any related decision model that informed this initiative
        /// </summary>
        public Guid? RelatedDecisionModelId { get; private set; }
        
        /// <summary>
        /// List of supporting analysis references related to this initiative
        /// </summary>
        private readonly List<AnalysisReference> _supportingAnalyses;
        
        /// <summary>
        /// Read-only collection of supporting analyses
        /// </summary>
        public IReadOnlyCollection<AnalysisReference> SupportingAnalyses => _supportingAnalyses.AsReadOnly();
        
        /// <summary>
        /// When the initiative was proposed
        /// </summary>
        public DateTime ProposedDate { get; private set; }
        
        /// <summary>
        /// When the initiative was approved or rejected (if applicable)
        /// </summary>
        public DateTime? DecisionDate { get; private set; }
        
        #endregion

        #region Constructors
        
        protected StrategicInitiativeProposal()
        {
            _supportingAnalyses = new List<AnalysisReference>();
        }
        
        /// <summary>
        /// Creates a new strategic initiative proposal
        /// </summary>
        public static StrategicInitiativeProposal Create(
            Guid id,
            string title,
            string description,
            DecisionRationaleText rationale,
            ExpectedOutcomeText expectedOutcomes,
            Guid? relatedDecisionModelId = null,
            AlignmentStatus strategicAlignment = AlignmentStatus.NotEvaluated,
            Guid? tenantId = null)
        {
            return new StrategicInitiativeProposal(
                id,
                title,
                description,
                rationale,
                expectedOutcomes,
                relatedDecisionModelId,
                strategicAlignment,
                tenantId);
        }
        
        private StrategicInitiativeProposal(
            Guid id,
            string title,
            string description,
            DecisionRationaleText rationale,
            ExpectedOutcomeText expectedOutcomes,
            Guid? relatedDecisionModelId,
            AlignmentStatus strategicAlignment,
            Guid? tenantId) : base(id)
        {
            SetTitle(title);
            SetDescription(description);
            Rationale = rationale ?? throw new ArgumentNullException(nameof(rationale));
            ExpectedOutcomes = expectedOutcomes ?? throw new ArgumentNullException(nameof(expectedOutcomes));
            RelatedDecisionModelId = relatedDecisionModelId;
            StrategicAlignment = strategicAlignment;
            TenantId = tenantId;
            Status = ProposalStatus.Proposed;
            ProposedDate = DateTime.UtcNow;
            _supportingAnalyses = new List<AnalysisReference>();
            
            AddDistributedEvent(new StrategicInitiativeProposedEto
            {
                Id = Id,
                Title = Title,
                TenantId = TenantId,
                ExpectedOutcomes = ExpectedOutcomes.Value,
                ProposerId = CreatorId ?? Guid.Empty
            });
        }
        
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Approves the initiative, which may lead to project creation
        /// </summary>
        /// <param name="approverId">ID of the user approving the initiative</param>
        /// <param name="projectId">Optional ID of the project created as a result</param>
        /// <param name="approvalTime">Time of approval</param>
        public void Approve(Guid approverId, Guid? projectId = null, DateTime? approvalTime = null)
        {
            if (Status == ProposalStatus.Approved)
            {
                throw new DecisionSupportDomainException(
                    "DecisionSupport:ErrorMessage:AlreadyApproved",
                    $"Initiative {Id} has already been approved");
            }
            
            if (Status == ProposalStatus.Rejected)
            {
                throw new InvalidInitiativeStatusTransitionException(Status, ProposalStatus.Approved);
            }
            
            Status = ProposalStatus.Approved;
            DecisionDate = approvalTime ?? DateTime.UtcNow;
            
            if (projectId.HasValue && projectId != Guid.Empty)
            {
                ProjectId = new ProjectId(projectId.Value);
            }
            
            AddDistributedEvent(new StrategicInitiativeApprovedEto
            {
                Id = Id,
                Title = Title,
                TenantId = TenantId,
                ProjectId = projectId,
                ApproverId = approverId,
                ApprovedOn = DecisionDate.Value
            });
        }
        
        /// <summary>
        /// Rejects the initiative with a reason
        /// </summary>
        /// <param name="rejectionReason">Reason for rejection</param>
        /// <param name="rejectorId">ID of the user rejecting the initiative</param>
        /// <param name="rejectionTime">Time of rejection</param>
        public void Reject(string rejectionReason, Guid rejectorId, DateTime? rejectionTime = null)
        {
            if (Status == ProposalStatus.Rejected)
            {
                throw new DecisionSupportDomainException(
                    "DecisionSupport:ErrorMessage:AlreadyRejected",
                    $"Initiative {Id} has already been rejected");
            }
            
            if (Status == ProposalStatus.Approved)
            {
                throw new InvalidInitiativeStatusTransitionException(Status, ProposalStatus.Rejected);
            }
            
            if (string.IsNullOrWhiteSpace(rejectionReason))
            {
                throw new DecisionSupportDomainException(
                    "DecisionSupport:ErrorMessage:RejectionReasonRequired",
                    "A reason is required when rejecting an initiative");
            }
            
            Status = ProposalStatus.Rejected;
            RejectionReason = rejectionReason;
            DecisionDate = rejectionTime ?? DateTime.UtcNow;
            
            AddDistributedEvent(new StrategicInitiativeRejectedEto
            {
                Id = Id,
                Title = Title,
                TenantId = TenantId,
                RejectionReason = RejectionReason,
                RejectorId = rejectorId,
                RejectedOn = DecisionDate.Value
            });
        }
        
        /// <summary>
        /// Updates the initiative details (only possible when in Proposed status)
        /// </summary>
        public void Update(
            string title,
            string description,
            DecisionRationaleText rationale,
            ExpectedOutcomeText expectedOutcomes,
            AlignmentStatus strategicAlignment,
            Guid? relatedDecisionModelId = null)
        {
            if (Status != ProposalStatus.Proposed)
            {
                throw new DecisionSupportDomainException(
                    "DecisionSupport:ErrorMessage:CannotUpdateAfterDecision",
                    $"Cannot update initiative after it has been {Status}");
            }
            
            SetTitle(title);
            SetDescription(description);
            Rationale = rationale ?? throw new ArgumentNullException(nameof(rationale));
            ExpectedOutcomes = expectedOutcomes ?? throw new ArgumentNullException(nameof(expectedOutcomes));
            StrategicAlignment = strategicAlignment;
            RelatedDecisionModelId = relatedDecisionModelId;
        }
        
        /// <summary>
        /// Adds a reference to supporting analysis
        /// </summary>
        public void AddSupportingAnalysis(
            string referenceType,
            string referenceKey,
            string referenceDescription)
        {
            if (_supportingAnalyses.Any(a => a.ReferenceKey == referenceKey && a.ReferenceType == referenceType))
            {
                return; // Already exists
            }
            
            var reference = new AnalysisReference(
                Id,
                referenceType,
                referenceKey,
                referenceDescription);
                
            _supportingAnalyses.Add(reference);
        }
        
        /// <summary>
        /// Removes a supporting analysis reference
        /// </summary>
        public void RemoveSupportingAnalysis(string referenceType, string referenceKey)
        {
            var reference = _supportingAnalyses.FirstOrDefault(
                a => a.ReferenceType == referenceType && a.ReferenceKey == referenceKey);
                
            if (reference != null)
            {
                _supportingAnalyses.Remove(reference);
            }
        }
        
        #endregion

        #region Private Methods
        
        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: DecisionSupportConsts.MaxTitleLength
            );
        }
        
        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: DecisionSupportConsts.MaxDescriptionLength
            );
        }
        
        #endregion
    }
}
