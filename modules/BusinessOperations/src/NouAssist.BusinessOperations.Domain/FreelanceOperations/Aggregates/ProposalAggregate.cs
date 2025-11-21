using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using NouAssist.BusinessOperations.FreelanceOperations.Events;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.FreelanceOperations.Aggregates
{
    /// <summary>
    /// Aggregate root representing a freelance proposal
    /// </summary>
    public class Proposal : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Title of the proposal
        /// </summary>
        public string Title { get; private set; }
        
        /// <summary>
        /// ID of the client this proposal is for
        /// </summary>
        public Guid ClientId { get; private set; }
        
        /// <summary>
        /// ID of the project this proposal is related to
        /// </summary>
        public Guid? ProjectId { get; private set; }
        
        /// <summary>
        /// Current status of the proposal
        /// </summary>
        public ProposalStatus Status { get; private set; }
        
        /// <summary>
        /// Template used for this proposal
        /// </summary>
        public ProposalTemplateReference TemplateReference { get; private set; }
        
        /// <summary>
        /// Date when the proposal was sent to the client
        /// </summary>
        public DateTime? SentDate { get; private set; }
        
        /// <summary>
        /// Date when the proposal was viewed by the client
        /// </summary>
        public DateTime? ViewedDate { get; private set; }
        
        /// <summary>
        /// Date when the client responded to the proposal
        /// </summary>
        public DateTime? ResponseDate { get; private set; }
        
        /// <summary>
        /// Expiration date for the proposal
        /// </summary>
        public DateTime? ExpirationDate { get; private set; }
        
        /// <summary>
        /// ID of the current active version of the proposal
        /// </summary>
        public Guid? CurrentVersionId { get; private set; }
        
        /// <summary>
        /// Collection of versions of this proposal
        /// </summary>
        public IReadOnlyCollection<ProposalVersion> Versions => _versions.AsReadOnly();
        private readonly List<ProposalVersion> _versions;

        protected Proposal()
        {
            _versions = new List<ProposalVersion>();
        }

        /// <summary>
        /// Creates a new proposal
        /// </summary>
        public static Proposal Create(
            Guid id,
            string title,
            Guid clientId,
            ProposalTemplateReference templateReference,
            Guid? projectId = null,
            DateTime? expirationDate = null,
            Guid? tenantId = null)
        {
            var proposal = new Proposal(
                id,
                title,
                clientId,
                templateReference,
                projectId,
                expirationDate,
                tenantId);
                
            return proposal;
        }

        private Proposal(
            Guid id,
            string title,
            Guid clientId,
            ProposalTemplateReference templateReference,
            Guid? projectId = null,
            DateTime? expirationDate = null,
            Guid? tenantId = null) : base(id)
        {
            Title = Check.NotNullOrWhiteSpace(
                title, 
                nameof(title), 
                maxLength: FreelanceOperationsConsts.Proposal.MaxTitleLength
            );
            
            ClientId = clientId;
            ProjectId = projectId;
            TemplateReference = templateReference ?? throw new ArgumentNullException(nameof(templateReference));
            ExpirationDate = expirationDate;
            TenantId = tenantId;
            Status = ProposalStatus.Draft;
            _versions = new List<ProposalVersion>();
            
            AddDistributedEvent(new ProposalCreatedEto
            {
                Id = Id,
                Title = Title,
                ProjectId = ProjectId ?? Guid.Empty,
                ClientId = ClientId,
                Status = Status,
                CreatedAt = DateTime.UtcNow,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Update the basic information for this proposal
        /// </summary>
        public void UpdateBasicInfo(
            string title,
            ProposalTemplateReference templateReference,
            DateTime? expirationDate = null)
        {
            // Can't update if not in draft or needs revision
            if (Status != ProposalStatus.Draft && Status != ProposalStatus.NeedsRevision)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            Title = Check.NotNullOrWhiteSpace(
                title, 
                nameof(title), 
                maxLength: FreelanceOperationsConsts.Proposal.MaxTitleLength
            );
            
            TemplateReference = templateReference ?? throw new ArgumentNullException(nameof(templateReference));
            ExpirationDate = expirationDate;
        }

        /// <summary>
        /// Add a new version to this proposal
        /// </summary>
        public ProposalVersion AddVersion(
            Guid versionId,
            string versionNumber,
            string versionName,
            ProposalContentSummary contentSummary,
            PricingStructure pricing,
            DocumentId documentId,
            Guid createdBy,
            string notes = null)
        {
            // Can't add versions if proposal is accepted, rejected, or expired
            if (Status != ProposalStatus.Draft && 
                Status != ProposalStatus.NeedsRevision && 
                Status != ProposalStatus.Sent &&
                Status != ProposalStatus.Viewed)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            var version = new ProposalVersion(
                versionId,
                Id,
                versionNumber,
                versionName,
                contentSummary,
                pricing,
                documentId,
                DateTime.UtcNow,
                createdBy,
                notes
            );
            
            _versions.Add(version);
            CurrentVersionId = versionId;
            
            // If sent or viewed, change status to needs revision since we've updated it
            if (Status == ProposalStatus.Sent || Status == ProposalStatus.Viewed)
            {
                Status = ProposalStatus.NeedsRevision;
            }
            
            return version;
        }

        /// <summary>
        /// Set the active version of the proposal
        /// </summary>
        public void SetCurrentVersion(Guid versionId)
        {
            var version = GetVersion(versionId);
            CurrentVersionId = versionId;
        }

        /// <summary>
        /// Send the proposal to the client
        /// </summary>
        public void Send(string sentToEmail, DateTime sentDate)
        {
            // Must have at least one version
            if (!CurrentVersionId.HasValue)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            // Can only send from draft or needs revision status
            if (Status != ProposalStatus.Draft && Status != ProposalStatus.NeedsRevision)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            Status = ProposalStatus.Sent;
            SentDate = sentDate;
            
            AddDistributedEvent(new ProposalSentEto
            {
                Id = Id,
                Title = Title,
                ProjectId = ProjectId ?? Guid.Empty,
                ClientId = ClientId,
                Status = Status,
                DocumentId = GetCurrentVersion().DocumentId.Value,
                SentAt = sentDate,
                SentToEmail = sentToEmail,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Mark the proposal as viewed by the client
        /// </summary>
        public void MarkAsViewed(DateTime viewedDate)
        {
            // Can only mark as viewed if currently sent
            if (Status != ProposalStatus.Sent)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            Status = ProposalStatus.Viewed;
            ViewedDate = viewedDate;
        }

        /// <summary>
        /// Mark the proposal as accepted by the client
        /// </summary>
        public void MarkAsAccepted(DateTime responseDate)
        {
            // Can only accept if sent or viewed
            if (Status != ProposalStatus.Sent && Status != ProposalStatus.Viewed)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            Status = ProposalStatus.Accepted;
            ResponseDate = responseDate;
        }

        /// <summary>
        /// Mark the proposal as rejected by the client
        /// </summary>
        public void MarkAsRejected(DateTime responseDate)
        {
            // Can only reject if sent or viewed
            if (Status != ProposalStatus.Sent && Status != ProposalStatus.Viewed)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            Status = ProposalStatus.Rejected;
            ResponseDate = responseDate;
        }

        /// <summary>
        /// Mark the proposal as needing revision
        /// </summary>
        public void MarkAsNeedsRevision()
        {
            // Can only mark as needs revision if sent, viewed, or draft
            if (Status != ProposalStatus.Sent && Status != ProposalStatus.Viewed && Status != ProposalStatus.Draft)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            Status = ProposalStatus.NeedsRevision;
        }

        /// <summary>
        /// Mark the proposal as expired
        /// </summary>
        public void MarkAsExpired(DateTime expirationDate)
        {
            // Can only expire if sent or viewed
            if (Status != ProposalStatus.Sent && Status != ProposalStatus.Viewed)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            Status = ProposalStatus.Expired;
            ResponseDate = expirationDate;
        }

        /// <summary>
        /// Get the current active version of the proposal
        /// </summary>
        public ProposalVersion GetCurrentVersion()
        {
            if (!CurrentVersionId.HasValue)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            return GetVersion(CurrentVersionId.Value);
        }

        /// <summary>
        /// Get a specific version of the proposal
        /// </summary>
        private ProposalVersion GetVersion(Guid versionId)
        {
            var version = _versions.FirstOrDefault(v => v.Id == versionId);
            
            if (version == null)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:EntityNotFound", "Proposal version");
            }
            
            return version;
        }

        /// <summary>
        /// Check if the proposal is expired based on its expiration date
        /// </summary>
        public bool IsExpired()
        {
            return ExpirationDate.HasValue && DateTime.UtcNow > ExpirationDate.Value;
        }

        /// <summary>
        /// Get the pricing information from the current version
        /// </summary>
        public PricingStructure GetCurrentPricing()
        {
            return GetCurrentVersion().Pricing;
        }
    }
}
