using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using NouAssist.BusinessOperations.FreelanceOperations.Exceptions;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services;
using NouAssist.BusinessOperations.FreelanceOperations.Specifications;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp.Users;

namespace NouAssist.BusinessOperations.FreelanceOperations.Services
{
    /// <summary>
    /// Domain service for managing proposals
    /// </summary>
    public class ProposalManager : DomainService, IProposalManager
    {
        private readonly IProposalRepository _proposalRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IFreelanceProjectRepository _projectRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<ProposalManager> _logger;

        public ProposalManager(
            IProposalRepository proposalRepository,
            IClientRepository clientRepository,
            IFreelanceProjectRepository projectRepository,
            IGuidGenerator guidGenerator,
            ICurrentUser currentUser,
            ILogger<ProposalManager> logger)
        {
            _proposalRepository = proposalRepository;
            _clientRepository = clientRepository;
            _projectRepository = projectRepository;
            _guidGenerator = guidGenerator;
            _currentUser = currentUser;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Proposal> CreateProposalAsync(
            string title,
            Guid clientId,
            ProposalTemplateReference templateReference,
            Guid? projectId = null,
            DateTime? expirationDate = null)
        {
            try
            {
                // Verify client exists
                if (!await _clientRepository.AnyAsync(clientId))
                {
                    throw new ClientNotFoundException(clientId);
                }

                // Verify project exists if provided
                if (projectId.HasValue && !await _projectRepository.AnyAsync(projectId.Value))
                {
                    throw new ProjectNotFoundException(projectId.Value);
                }

                var proposalId = _guidGenerator.Create();
                
                var proposal = Proposal.Create(
                    proposalId,
                    title,
                    clientId,
                    templateReference,
                    projectId,
                    expirationDate,
                    CurrentTenant.Id
                );

                await _proposalRepository.InsertAsync(proposal);
                return proposal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create proposal with title {Title}", title);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ProposalVersion> AddProposalVersionAsync(
            Guid proposalId,
            string versionNumber,
            string versionName,
            ProposalContentSummary contentSummary,
            PricingStructure pricing,
            DocumentId documentId,
            string notes = null)
        {
            try
            {
                var proposal = await _proposalRepository.GetAsync(proposalId);
                
                var userId = _currentUser.Id ?? throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
                
                var versionId = _guidGenerator.Create();
                var version = proposal.AddVersion(
                    versionId,
                    versionNumber,
                    versionName,
                    contentSummary,
                    pricing,
                    documentId,
                    userId.Value,
                    notes
                );

                await _proposalRepository.UpdateAsync(proposal);
                return version;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add version to proposal {ProposalId}", proposalId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task SendProposalAsync(
            Guid proposalId,
            string sentToEmail)
        {
            try
            {
                var proposal = await _proposalRepository.GetAsync(proposalId);
                
                proposal.Send(sentToEmail, DateTime.UtcNow);

                await _proposalRepository.UpdateAsync(proposal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send proposal {ProposalId}", proposalId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task AcceptProposalAsync(
            Guid proposalId)
        {
            try
            {
                var proposal = await _proposalRepository.GetAsync(proposalId);
                
                proposal.MarkAsAccepted(DateTime.UtcNow);

                // If this proposal is linked to a project, update the project
                if (proposal.ProjectId.HasValue)
                {
                    var project = await _projectRepository.GetAsync(proposal.ProjectId.Value);
                    project.LinkToProposal(proposalId);
                    
                    // If the project is in draft or proposed status, change to active
                    if (project.Status == ProjectStatus.Draft || project.Status == ProjectStatus.Proposed)
                    {
                        project.ChangeStatus(ProjectStatus.Active, DateTime.UtcNow);
                    }
                    
                    await _projectRepository.UpdateAsync(project);
                }

                await _proposalRepository.UpdateAsync(proposal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to accept proposal {ProposalId}", proposalId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task RejectProposalAsync(
            Guid proposalId)
        {
            try
            {
                var proposal = await _proposalRepository.GetAsync(proposalId);
                
                proposal.MarkAsRejected(DateTime.UtcNow);

                await _proposalRepository.UpdateAsync(proposal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reject proposal {ProposalId}", proposalId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task MarkProposalAsNeedsRevisionAsync(
            Guid proposalId)
        {
            try
            {
                var proposal = await _proposalRepository.GetAsync(proposalId);
                
                proposal.MarkAsNeedsRevision();

                await _proposalRepository.UpdateAsync(proposal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mark proposal {ProposalId} as needing revision", proposalId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<Proposal>> GetProposalsNearingExpirationAsync(int daysUntilExpiration = 3)
        {
            try
            {
                var specification = new ProposalsNearingExpirationSpecification(daysUntilExpiration);
                return await _proposalRepository.GetListAsync(specification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get proposals nearing expiration");
                throw;
            }
        }
    }
}
