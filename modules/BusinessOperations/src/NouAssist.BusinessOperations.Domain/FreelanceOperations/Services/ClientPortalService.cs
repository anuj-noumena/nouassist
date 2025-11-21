using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services;
using NouAssist.BusinessOperations.FreelanceOperations.Specifications;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.FreelanceOperations.Services
{
    /// <summary>
    /// Domain service for client portal operations
    /// </summary>
    public class ClientPortalService : DomainService, IClientPortalService
    {
        private readonly IFreelanceProjectRepository _projectRepository;
        private readonly IProposalRepository _proposalRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientPortalService> _logger;

        public ClientPortalService(
            IFreelanceProjectRepository projectRepository,
            IProposalRepository proposalRepository,
            IClientRepository clientRepository,
            ILogger<ClientPortalService> logger)
        {
            _projectRepository = projectRepository;
            _proposalRepository = proposalRepository;
            _clientRepository = clientRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, object>> GetProjectProgressAsync(Guid projectId, Guid clientId)
        {
            try
            {
                var project = await _projectRepository.GetAsync(projectId);
                
                // Verify this project belongs to the specified client
                if (project.ClientId != clientId)
                {
                    throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
                }
                
                // Prepare project progress data for client portal
                var result = new Dictionary<string, object>
                {
                    ["projectId"] = project.Id,
                    ["name"] = project.Name,
                    ["status"] = project.Status.ToString(),
                    ["startDate"] = project.StartDate,
                    ["endDate"] = project.EndDate,
                    ["completionPercentage"] = CalculateProjectCompletion(project)
                };
                
                if (project.BillingType == BillingType.Milestone)
                {
                    var milestones = project.Milestones
                        .Select(m => new Dictionary<string, object>
                        {
                            ["id"] = m.Id,
                            ["title"] = m.Details.Title,
                            ["isCompleted"] = m.IsCompleted,
                            ["completedDate"] = m.CompletedDate,
                            ["deadline"] = m.Deadline,
                            ["isOverdue"] = m.IsOverdue()
                        })
                        .ToList();
                    
                    result["milestones"] = milestones;
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get project progress for client {ClientId}, project {ProjectId}", 
                    clientId, projectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task RecordClientProjectInteractionAsync(Guid projectId, Guid clientId, string interactionType)
        {
            try
            {
                var project = await _projectRepository.GetAsync(projectId);
                
                // Verify this project belongs to the specified client
                if (project.ClientId != clientId)
                {
                    throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
                }
                
                // Record interaction (could log to database or create event for analytics)
                _logger.LogInformation(
                    "Client {ClientId} interaction with project {ProjectId}: {InteractionType}", 
                    clientId, projectId, interactionType);
                
                // If this was a project view, could update client activity timestamp in a separate service
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record client interaction for client {ClientId}, project {ProjectId}", 
                    clientId, projectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<Dictionary<string, object>>> GetClientProposalsAsync(Guid clientId)
        {
            try
            {
                var client = await _clientRepository.GetAsync(clientId);
                
                var specification = new ProposalsForClientSpecification(clientId);
                var proposals = await _proposalRepository.GetListAsync(specification);
                
                // Prepare proposals data for client portal
                var result = proposals
                    .Select(p => new Dictionary<string, object>
                    {
                        ["proposalId"] = p.Id,
                        ["title"] = p.Title,
                        ["status"] = p.Status.ToString(),
                        ["sentDate"] = p.SentDate,
                        ["expirationDate"] = p.ExpirationDate,
                        ["isExpired"] = p.IsExpired(),
                        ["projectName"] = p.ProjectId.HasValue ? GetProjectName(p.ProjectId.Value) : null
                    })
                    .ToList();
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get proposals for client {ClientId}", clientId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task RecordProposalViewedAsync(Guid proposalId, Guid clientId)
        {
            try
            {
                var proposal = await _proposalRepository.GetAsync(proposalId);
                
                // Verify this proposal belongs to the specified client
                if (proposal.ClientId != clientId)
                {
                    throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
                }
                
                // Mark proposal as viewed
                if (proposal.Status == ProposalStatus.Sent)
                {
                    proposal.MarkAsViewed(DateTime.UtcNow);
                    await _proposalRepository.UpdateAsync(proposal);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record proposal viewed for client {ClientId}, proposal {ProposalId}", 
                    clientId, proposalId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task RecordProposalResponseAsync(Guid proposalId, Guid clientId, bool accepted)
        {
            try
            {
                var proposal = await _proposalRepository.GetAsync(proposalId);
                
                // Verify this proposal belongs to the specified client
                if (proposal.ClientId != clientId)
                {
                    throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
                }
                
                // Record client response
                if (accepted)
                {
                    proposal.MarkAsAccepted(DateTime.UtcNow);
                    
                    // If proposal is linked to a project, update the project
                    if (proposal.ProjectId.HasValue)
                    {
                        var project = await _projectRepository.GetAsync(proposal.ProjectId.Value);
                        project.LinkToProposal(proposalId);
                        
                        // If project is in draft or proposed status, change to active
                        if (project.Status == ProjectStatus.Draft || project.Status == ProjectStatus.Proposed)
                        {
                            project.ChangeStatus(ProjectStatus.Active, DateTime.UtcNow);
                        }
                        
                        await _projectRepository.UpdateAsync(project);
                    }
                }
                else
                {
                    proposal.MarkAsRejected(DateTime.UtcNow);
                }
                
                await _proposalRepository.UpdateAsync(proposal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record proposal response for client {ClientId}, proposal {ProposalId}", 
                    clientId, proposalId);
                throw;
            }
        }
        
        private double CalculateProjectCompletion(FreelanceProject project)
        {
            switch (project.Status)
            {
                case ProjectStatus.Draft:
                    return 0;
                case ProjectStatus.Proposed:
                    return 10;
                case ProjectStatus.Completed:
                case ProjectStatus.Archived:
                    return 100;
                case ProjectStatus.Canceled:
                    return 0;
                case ProjectStatus.Active:
                case ProjectStatus.OnHold:
                    if (project.BillingType == BillingType.Milestone)
                    {
                        if (project.Milestones.Count == 0)
                            return 0;
                            
                        return (double)project.Milestones.Count(m => m.IsCompleted) / project.Milestones.Count * 100;
                    }
                    
                    // For hourly or fixed price, estimate based on start/end dates
                    if (project.StartDate.HasValue && project.EndDate.HasValue)
                    {
                        var totalDays = (project.EndDate.Value - project.StartDate.Value).TotalDays;
                        if (totalDays <= 0)
                            return 0;
                            
                        var daysPassed = (DateTime.UtcNow - project.StartDate.Value).TotalDays;
                        var completion = daysPassed / totalDays * 100;
                        
                        return Math.Min(Math.Max(completion, 0), 99); // Cap at 99% until marked complete
                    }
                    
                    return 50; // Default to 50% if no reliable estimate is possible
                default:
                    return 0;
            }
        }
        
        private async Task<string> GetProjectName(Guid projectId)
        {
            try
            {
                var project = await _projectRepository.GetAsync(projectId);
                return project.Name;
            }
            catch
            {
                return "Unknown Project";
            }
        }
    }
}
