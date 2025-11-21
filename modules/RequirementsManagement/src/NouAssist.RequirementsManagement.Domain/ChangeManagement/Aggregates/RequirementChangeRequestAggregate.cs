using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;
using NouAssist.RequirementsManagement.ChangeManagement.Entities;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using NouAssist.RequirementsManagement.ChangeManagement.Enums;
using NouAssist.RequirementsManagement.ChangeManagement.Events;
using NouAssist.RequirementsManagement.ChangeManagement.Exceptions;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.ChangeManagement.Aggregates
{
    /// <summary>
    /// Represents a requirement change request with its approval workflow.
    /// Enforces business rules for change management process.
    /// </summary>
    public class RequirementChangeRequest : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public RequestIdentifier RequestIdentifier { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public AffectedRequirementRef AffectedRequirement { get; private set; }
        public RequestStatus Status { get; private set; }
        public Guid SubmitterId { get; private set; }
        public DateTime SubmissionDate { get; private set; }
        public DateTime? CompletionDate { get; private set; }

        // Navigation properties for related entities within the aggregate
        private readonly List<ChangeRationale> _rationales;
        private readonly List<ChangeImpactStatement> _impactStatements;
        private readonly List<ApprovalStep> _approvalSteps;

        // Public read-only access to collections
        public IReadOnlyCollection<ChangeRationale> Rationales => _rationales.AsReadOnly();
        public IReadOnlyCollection<ChangeImpactStatement> ImpactStatements => _impactStatements.AsReadOnly();
        public IReadOnlyCollection<ApprovalStep> ApprovalSteps => _approvalSteps.AsReadOnly();

        protected RequirementChangeRequest()
        {
            _rationales = new List<ChangeRationale>();
            _impactStatements = new List<ChangeImpactStatement>();
            _approvalSteps = new List<ApprovalStep>();
        }

        /// <summary>
        /// Factory method for creating a new change request
        /// </summary>
        public static RequirementChangeRequest Create(
            Guid id,
            RequestIdentifier requestIdentifier,
            string title,
            string description,
            AffectedRequirementRef affectedRequirement,
            Guid submitterId,
            DateTime submissionDate,
            Guid? tenantId = null)
        {
            return new RequirementChangeRequest(
                id,
                requestIdentifier,
                title,
                description,
                affectedRequirement,
                submitterId,
                submissionDate,
                tenantId
            );
        }

        private RequirementChangeRequest(
            Guid id,
            RequestIdentifier requestIdentifier,
            string title,
            string description,
            AffectedRequirementRef affectedRequirement,
            Guid submitterId,
            DateTime submissionDate,
            Guid? tenantId) : base(id)
        {
            RequestIdentifier = Check.NotNull(requestIdentifier, nameof(requestIdentifier));
            SetTitle(title);
            SetDescription(description);
            AffectedRequirement = Check.NotNull(affectedRequirement, nameof(affectedRequirement));
            SubmitterId = Check.NotEmpty(submitterId, nameof(submitterId));
            SubmissionDate = submissionDate;
            TenantId = tenantId;
            Status = RequestStatus.Submitted;

            _rationales = new List<ChangeRationale>();
            _impactStatements = new List<ChangeImpactStatement>();
            _approvalSteps = new List<ApprovalStep>();

            // Publish creation event
            AddDistributedEvent(new ChangeRequestCreatedEto
            {
                Id = Id,
                RequestIdentifier = RequestIdentifier.Value,
                Title = Title,
                Status = Status,
                SubmitterId = SubmitterId,
                AffectedRequirement = AffectedRequirement,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates the title of the change request
        /// </summary>
        public void UpdateTitle(string title)
        {
            EnsureCanModify();
            SetTitle(title);
        }

        /// <summary>
        /// Updates the description of the change request
        /// </summary>
        public void UpdateDescription(string description)
        {
            EnsureCanModify();
            SetDescription(description);
        }

        /// <summary>
        /// Adds a rationale for the change request
        /// </summary>
        public void AddRationale(string rationaleText, string category = null)
        {
            EnsureCanModify();

            var rationale = new ChangeRationale(Id, rationaleText, category);
            _rationales.Add(rationale);
        }

        /// <summary>
        /// Removes a rationale from the change request
        /// </summary>
        public void RemoveRationale(Guid rationaleId)
        {
            EnsureCanModify();

            var rationale = _rationales.FirstOrDefault(r => r.Id == rationaleId);
            if (rationale != null)
            {
                _rationales.Remove(rationale);
            }
        }

        /// <summary>
        /// Adds an impact statement to the change request
        /// </summary>
        public void AddImpactStatement(ImpactDetails impactDetails, string description)
        {
            EnsureCanModify();

            var impactStatement = new ChangeImpactStatement(Id, impactDetails, description);
            _impactStatements.Add(impactStatement);
        }

        /// <summary>
        /// Removes an impact statement from the change request
        /// </summary>
        public void RemoveImpactStatement(Guid impactStatementId)
        {
            EnsureCanModify();

            var statement = _impactStatements.FirstOrDefault(i => i.Id == impactStatementId);
            if (statement != null)
            {
                _impactStatements.Remove(statement);
            }
        }

        /// <summary>
        /// Adds an approval step to the change request
        /// </summary>
        public void AddApprovalStep(ApproverRole approverRole, Guid approverId, int sequenceOrder)
        {
            if (Status != RequestStatus.Submitted && Status != RequestStatus.UnderReview)
            {
                throw new InvalidStatusTransitionException(Status, RequestStatus.PendingApproval);
            }

            if (_approvalSteps.Count >= ChangeManagementConsts.MaxApprovalSteps)
            {
                throw new ChangeManagementDomainException("ChangeManagement:ErrorMessage:MaxApprovalStepsExceeded");
            }

            // Ensure unique sequence order
            if (_approvalSteps.Any(a => a.SequenceOrder == sequenceOrder))
            {
                throw new ChangeManagementDomainException("ChangeManagement:ErrorMessage:DuplicateSequenceOrder");
            }

            var approvalStep = new ApprovalStep(Id, approverRole, approverId, sequenceOrder);
            _approvalSteps.Add(approvalStep);
        }

        /// <summary>
        /// Processes an approval decision for a specific approval step
        /// </summary>
        public void ProcessApproval(Guid approvalStepId, ApprovalDecision decision, DecisionReason reason, DateTime decisionDate)
        {
            if (Status != RequestStatus.PendingApproval)
            {
                throw new InvalidStatusTransitionException(Status, RequestStatus.PendingApproval);
            }

            var approvalStep = _approvalSteps.FirstOrDefault(a => a.Id == approvalStepId);
            if (approvalStep == null)
            {
                throw new ChangeManagementDomainException("ChangeManagement:ErrorMessage:ApprovalStepNotFound");
            }

            // Process the approval decision
            approvalStep.ProcessDecision(decision, reason, decisionDate);

            // Evaluate overall approval status
            EvaluateApprovalStatus();

            // Publish appropriate events
            if (decision == ApprovalDecision.Approved)
            {
                AddDistributedEvent(new ChangeRequestApprovedEto
                {
                    Id = Id,
                    RequestIdentifier = RequestIdentifier.Value,
                    Title = Title,
                    Status = Status,
                    ApprovalStepId = approvalStepId,
                    ApproverId = approvalStep.ApproverId,
                    ApproverRole = approvalStep.ApproverRole,
                    DecisionReason = reason.Value,
                    ApprovalDate = decisionDate,
                    TenantId = TenantId
                });
            }
            else if (decision == ApprovalDecision.Rejected)
            {
                AddDistributedEvent(new ChangeRequestRejectedEto
                {
                    Id = Id,
                    RequestIdentifier = RequestIdentifier.Value,
                    Title = Title,
                    Status = Status,
                    ApprovalStepId = approvalStepId,
                    ApproverId = approvalStep.ApproverId,
                    ApproverRole = approvalStep.ApproverRole,
                    DecisionReason = reason.Value,
                    RejectionDate = decisionDate,
                    TenantId = TenantId
                });
            }
        }

        /// <summary>
        /// Transitions the change request to under review status
        /// </summary>
        public void MoveToUnderReview()
        {
            ValidateStatusTransition(RequestStatus.UnderReview);
            ChangeStatus(RequestStatus.UnderReview);
        }

        /// <summary>
        /// Transitions the change request to impact assessment status
        /// </summary>
        public void MoveToImpactAssessment()
        {
            ValidateStatusTransition(RequestStatus.ImpactAssessment);
            ChangeStatus(RequestStatus.ImpactAssessment);
        }

        /// <summary>
        /// Transitions the change request to pending approval status
        /// </summary>
        public void MoveToPendingApproval()
        {
            ValidateStatusTransition(RequestStatus.PendingApproval);

            if (!_approvalSteps.Any())
            {
                throw new ChangeManagementDomainException("ChangeManagement:ErrorMessage:NoApprovalSteps");
            }

            ChangeStatus(RequestStatus.PendingApproval);
        }

        /// <summary>
        /// Marks the change request as in progress
        /// </summary>
        public void StartImplementation()
        {
            ValidateStatusTransition(RequestStatus.InProgress);
            ChangeStatus(RequestStatus.InProgress);
        }

        /// <summary>
        /// Marks the change request as completed
        /// </summary>
        public void MarkAsCompleted(DateTime completionDate)
        {
            ValidateStatusTransition(RequestStatus.Completed);
            CompletionDate = completionDate;
            ChangeStatus(RequestStatus.Completed);
        }

        /// <summary>
        /// Cancels the change request
        /// </summary>
        public void Cancel(string reason = null)
        {
            if (Status == RequestStatus.Completed)
            {
                throw new ChangeManagementDomainException("ChangeManagement:ErrorMessage:CannotCancelCompleted");
            }

            ChangeStatus(RequestStatus.Cancelled);
        }

        /// <summary>
        /// Gets the next approval step that requires a decision
        /// </summary>
        public ApprovalStep GetNextPendingApprovalStep()
        {
            return _approvalSteps
                .Where(a => a.Decision == ApprovalDecision.Pending)
                .OrderBy(a => a.SequenceOrder)
                .FirstOrDefault();
        }

        /// <summary>
        /// Checks if all required approvals have been received
        /// </summary>
        public bool HasAllRequiredApprovals()
        {
            return _approvalSteps.Any() && 
                   _approvalSteps.All(a => a.Decision == ApprovalDecision.Approved);
        }

        /// <summary>
        /// Checks if any approval has been rejected
        /// </summary>
        public bool HasAnyRejection()
        {
            return _approvalSteps.Any(a => a.Decision == ApprovalDecision.Rejected);
        }

        /// <summary>
        /// Gets the total estimated impact (combining all impact statements)
        /// </summary>
        public (decimal? TotalCostImpact, int? TotalTimeImpact) GetTotalImpact()
        {
            var totalCost = _impactStatements
                .Where(i => i.ImpactDetails.HasCostImpact)
                .Sum(i => i.ImpactDetails.CostImpact);

            var totalTime = _impactStatements
                .Where(i => i.ImpactDetails.HasTimeImpact)
                .Sum(i => i.ImpactDetails.TimeImpactDays);

            return (totalCost == 0 ? null : totalCost, totalTime == 0 ? null : totalTime);
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: ChangeManagementConsts.MaxTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ChangeManagementConsts.MaxDescriptionLength
            );
        }

        private void ValidateStatusTransition(RequestStatus targetStatus)
        {
            if (!IsValidStatusTransition(Status, targetStatus))
            {
                throw new InvalidStatusTransitionException(Status, targetStatus);
            }
        }

        private static bool IsValidStatusTransition(RequestStatus currentStatus, RequestStatus targetStatus)
        {
            return currentStatus switch
            {
                RequestStatus.Submitted => targetStatus is RequestStatus.UnderReview or RequestStatus.Cancelled,
                RequestStatus.UnderReview => targetStatus is RequestStatus.ImpactAssessment or RequestStatus.PendingApproval or RequestStatus.Cancelled,
                RequestStatus.ImpactAssessment => targetStatus is RequestStatus.PendingApproval or RequestStatus.UnderReview or RequestStatus.Cancelled,
                RequestStatus.PendingApproval => targetStatus is RequestStatus.Approved or RequestStatus.Rejected or RequestStatus.Cancelled,
                RequestStatus.Approved => targetStatus is RequestStatus.InProgress or RequestStatus.Cancelled,
                RequestStatus.InProgress => targetStatus is RequestStatus.Completed or RequestStatus.Cancelled,
                RequestStatus.Rejected => targetStatus is RequestStatus.UnderReview or RequestStatus.Cancelled,
                RequestStatus.Completed => false, // No transitions allowed from completed
                RequestStatus.Cancelled => false, // No transitions allowed from cancelled
                _ => false
            };
        }

        private void ChangeStatus(RequestStatus newStatus)
        {
            var previousStatus = Status;
            Status = newStatus;

            AddDistributedEvent(new ChangeRequestStatusChangedEto
            {
                Id = Id,
                RequestIdentifier = RequestIdentifier.Value,
                Title = Title,
                Status = Status,
                PreviousStatus = previousStatus,
                TenantId = TenantId
            });
        }

        private void EvaluateApprovalStatus()
        {
            if (HasAnyRejection())
            {
                ChangeStatus(RequestStatus.Rejected);
            }
            else if (HasAllRequiredApprovals())
            {
                ChangeStatus(RequestStatus.Approved);
            }
            // If neither all approved nor any rejected, stay in PendingApproval
        }

        private void EnsureCanModify()
        {
            if (Status is RequestStatus.Completed or RequestStatus.Cancelled)
            {
                throw new ChangeManagementDomainException("ChangeManagement:ErrorMessage:CannotModifyCompletedRequest");
            }
        }
    }
}
