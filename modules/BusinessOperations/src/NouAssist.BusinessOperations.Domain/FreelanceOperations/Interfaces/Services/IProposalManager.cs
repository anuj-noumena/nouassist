using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;

namespace NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services
{
    /// <summary>
    /// Domain service interface for managing proposals
    /// </summary>
    public interface IProposalManager
    {
        /// <summary>
        /// Creates a new proposal
        /// </summary>
        Task<Proposal> CreateProposalAsync(
            string title,
            Guid clientId,
            ProposalTemplateReference templateReference,
            Guid? projectId = null,
            DateTime? expirationDate = null);
        
        /// <summary>
        /// Adds a new version to a proposal
        /// </summary>
        Task<ProposalVersion> AddProposalVersionAsync(
            Guid proposalId,
            string versionNumber,
            string versionName,
            ProposalContentSummary contentSummary,
            PricingStructure pricing,
            DocumentId documentId,
            string notes = null);
        
        /// <summary>
        /// Sends a proposal to a client
        /// </summary>
        Task SendProposalAsync(
            Guid proposalId,
            string sentToEmail);
        
        /// <summary>
        /// Marks a proposal as accepted
        /// </summary>
        Task AcceptProposalAsync(
            Guid proposalId);
        
        /// <summary>
        /// Marks a proposal as rejected
        /// </summary>
        Task RejectProposalAsync(
            Guid proposalId);
        
        /// <summary>
        /// Marks a proposal as needing revision
        /// </summary>
        Task MarkProposalAsNeedsRevisionAsync(
            Guid proposalId);
        
        /// <summary>
        /// Gets proposals that are about to expire
        /// </summary>
        Task<List<Proposal>> GetProposalsNearingExpirationAsync(int daysUntilExpiration = 3);
    }
}
