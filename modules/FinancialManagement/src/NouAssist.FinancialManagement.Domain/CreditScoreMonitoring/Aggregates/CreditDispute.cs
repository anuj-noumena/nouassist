using System;
using NouAssist.CreditScoreMonitoring.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates
{
    public class CreditDispute : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public CreditBureau Bureau { get; private set; }
        public string ItemType { get; private set; }
        public string ItemDescription { get; private set; }
        public string DisputeReason { get; private set; }
        public string SupportingDocumentation { get; private set; }
        public DisputeStatus Status { get; private set; }
        public DateTime SubmittedDate { get; private set; }
        public DateTime? ResolutionDate { get; private set; }
        public string? ResolutionNotes { get; private set; }
        public string? ConfirmationNumber { get; private set; }

        protected CreditDispute() { /* ORM */ }

        public CreditDispute(
            Guid id,
            Guid userId,
            Guid? tenantId,
            CreditBureau bureau,
            string itemType,
            string itemDescription,
            string disputeReason,
            string supportingDocumentation)
            : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            Bureau = bureau;
            ItemType = Check.NotNullOrWhiteSpace(itemType, nameof(itemType));
            ItemDescription = Check.NotNullOrWhiteSpace(itemDescription, nameof(itemDescription));
            DisputeReason = Check.NotNullOrWhiteSpace(disputeReason, nameof(disputeReason));
            SupportingDocumentation = supportingDocumentation;
            Status = DisputeStatus.Submitted;
            SubmittedDate = DateTime.UtcNow;
        }

        public void MarkAsUnderInvestigation(string confirmationNumber)
        {
            if (Status != DisputeStatus.Submitted)
            {
                throw new BusinessException("CreditScoreMonitoring:InvalidDisputeStatus");
            }

            Status = DisputeStatus.UnderInvestigation;
            ConfirmationNumber = Check.NotNullOrWhiteSpace(confirmationNumber, nameof(confirmationNumber));
        }

        public void Resolve(string resolutionNotes)
        {
            if (Status != DisputeStatus.UnderInvestigation)
            {
                throw new BusinessException("CreditScoreMonitoring:DisputeMustBeUnderInvestigation");
            }

            Status = DisputeStatus.Resolved;
            ResolutionDate = DateTime.UtcNow;
            ResolutionNotes = Check.NotNullOrWhiteSpace(resolutionNotes, nameof(resolutionNotes));
        }

        public void Reject(string rejectionReason)
        {
            if (Status != DisputeStatus.UnderInvestigation)
            {
                throw new BusinessException("CreditScoreMonitoring:DisputeMustBeUnderInvestigation");
            }

            Status = DisputeStatus.Rejected;
            ResolutionDate = DateTime.UtcNow;
            ResolutionNotes = Check.NotNullOrWhiteSpace(rejectionReason, nameof(rejectionReason));
        }

        public void Cancel()
        {
            if (Status == DisputeStatus.Resolved || Status == DisputeStatus.Rejected)
            {
                throw new BusinessException("CreditScoreMonitoring:CannotCancelClosedDispute");
            }

            Status = DisputeStatus.Cancelled;
            ResolutionDate = DateTime.UtcNow;
        }
    }
}
