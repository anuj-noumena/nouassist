using System;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.FreelanceOperations.Entities
{
    /// <summary>
    /// Entity representing a version of a proposal
    /// </summary>
    public class ProposalVersion : Entity<Guid>
    {
        /// <summary>
        /// ID of the proposal this version belongs to
        /// </summary>
        public Guid ProposalId { get; private set; }
        
        /// <summary>
        /// Version number (e.g., 1.0, 1.1, 2.0)
        /// </summary>
        public string VersionNumber { get; private set; }
        
        /// <summary>
        /// Version name or label
        /// </summary>
        public string VersionName { get; private set; }
        
        /// <summary>
        /// Content summary for this version
        /// </summary>
        public ProposalContentSummary ContentSummary { get; private set; }
        
        /// <summary>
        /// Pricing structure for this version
        /// </summary>
        public PricingStructure Pricing { get; private set; }
        
        /// <summary>
        /// Document ID reference for this version
        /// </summary>
        public DocumentId DocumentId { get; private set; }
        
        /// <summary>
        /// Creation date of this version
        /// </summary>
        public DateTime CreatedAt { get; private set; }
        
        /// <summary>
        /// User ID who created this version
        /// </summary>
        public Guid CreatedBy { get; private set; }
        
        /// <summary>
        /// Any notes or comments about this version
        /// </summary>
        public string Notes { get; private set; }

        protected ProposalVersion()
        {
        }

        internal ProposalVersion(
            Guid id,
            Guid proposalId,
            string versionNumber,
            string versionName,
            ProposalContentSummary contentSummary,
            PricingStructure pricing,
            DocumentId documentId,
            DateTime createdAt,
            Guid createdBy,
            string notes = null)
            : base(id)
        {
            ProposalId = proposalId;
            VersionNumber = Check.NotNullOrWhiteSpace(versionNumber, nameof(versionNumber));
            
            VersionName = Check.NotNullOrWhiteSpace(
                versionName, 
                nameof(versionName), 
                maxLength: FreelanceOperationsConsts.Proposal.MaxVersionNameLength
            );
            
            ContentSummary = contentSummary ?? throw new ArgumentNullException(nameof(contentSummary));
            Pricing = pricing ?? throw new ArgumentNullException(nameof(pricing));
            DocumentId = documentId ?? throw new ArgumentNullException(nameof(documentId));
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            Notes = notes;
        }

        /// <summary>
        /// Update the notes for this version
        /// </summary>
        internal void UpdateNotes(string notes)
        {
            Notes = notes;
        }
    }
}
