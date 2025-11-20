using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.ClientPortal.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace NouAssist.BusinessOperations.FreelanceOperations.ClientPortal
{
    [Authorize]
    public class ClientPortalAppService : ApplicationService, IClientPortalAppService
    {
        private readonly IFreelanceProjectRepository _projectRepository;
        private readonly IProposalRepository _proposalRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IClientPortalService _clientPortalService;
        private readonly ICurrentUser _currentUser;
        
        public ClientPortalAppService(
            IFreelanceProjectRepository projectRepository,
            IProposalRepository proposalRepository,
            IClientRepository clientRepository,
            IClientPortalService clientPortalService,
            ICurrentUser currentUser)
        {
            _projectRepository = projectRepository;
            _proposalRepository = proposalRepository;
            _clientRepository = clientRepository;
            _clientPortalService = clientPortalService;
            _currentUser = currentUser;
            
            LocalizationResource = typeof(FreelanceOperationsResource);
        }

        public virtual async Task<ClientPortalProjectDto> GetProjectProgressAsync(Guid projectId)
        {
            try
            {
                // Get client ID for current user
                var clientId = await GetClientIdForCurrentUserAsync();
                
                // Get project progress using domain service
                var progressData = await _clientPortalService.GetProjectProgressAsync(projectId, clientId);
                
                // Convert to DTO
                var projectDto = new ClientPortalProjectDto
                {
                    Id = (Guid)progressData["projectId"],
                    Name = (string)progressData["name"],
                    Status = (Enums.ProjectStatus)progressData["status"],
                    StartDate = (DateTime?)progressData["startDate"],
                    EndDate = (DateTime?)progressData["endDate"],
                    CompletionPercentage = (double)progressData["completionPercentage"]
                };
                
                // Add milestones if available
                if (progressData.ContainsKey("milestones"))
                {
                    var milestonesData = (List<Dictionary<string, object>>)progressData["milestones"];
                    projectDto.Milestones = new List<ClientPortalMilestoneDto>();
                    
                    foreach (var milestoneData in milestonesData)
                    {
                        projectDto.Milestones.Add(new ClientPortalMilestoneDto
                        {
                            Id = (Guid)milestoneData["id"],
                            Title = (string)milestoneData["title"],
                            IsCompleted = (bool)milestoneData["isCompleted"],
                            CompletedDate = (DateTime?)milestoneData["completedDate"],
                            Deadline = (DateTime?)milestoneData["deadline"],
                            IsOverdue = (bool)milestoneData["isOverdue"]
                        });
                    }
                }
                
                return projectDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ProjectNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetProjectProgressAsync for project {ProjectId}", projectId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        public virtual async Task RecordProjectInteractionAsync(Guid projectId, ProjectInteractionDto input)
        {
            try
            {
                // Get client ID for current user
                var clientId = await GetClientIdForCurrentUserAsync();
                
                // Record interaction using domain service
                await _clientPortalService.RecordClientProjectInteractionAsync(
                    projectId,
                    clientId,
                    input.InteractionType);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ProjectNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in RecordProjectInteractionAsync for project {ProjectId}", projectId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        public virtual async Task<List<ClientPortalProposalDto>> GetClientProposalsAsync()
        {
            try
            {
                // Get client ID for current user
                var clientId = await GetClientIdForCurrentUserAsync();
                
                // Get proposals using domain service
                var proposalsData = await _clientPortalService.GetClientProposalsAsync(clientId);
                
                // Convert to DTOs
                var proposalDtos = new List<ClientPortalProposalDto>();
                
                foreach (var proposalData in proposalsData)
                {
                    proposalDtos.Add(new ClientPortalProposalDto
                    {
                        Id = (Guid)proposalData["proposalId"],
                        Title = (string)proposalData["title"],
                        Status = (Enums.ProposalStatus)proposalData["status"],
                        SentDate = (DateTime?)proposalData["sentDate"],
                        ExpirationDate = (DateTime?)proposalData["expirationDate"],
                        IsExpired = (bool)proposalData["isExpired"],
                        ProjectId = proposalData.ContainsKey("projectId") ? (Guid?)proposalData["projectId"] : null,
                        ProjectName = proposalData.ContainsKey("projectName") ? (string)proposalData["projectName"] : null,
                        DocumentId = proposalData.ContainsKey("documentId") ? (string)proposalData["documentId"] : null
                    });
                }
                
                return proposalDtos;
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetClientProposalsAsync");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        public virtual async Task RecordProposalViewedAsync(Guid proposalId)
        {
            try
            {
                // Get client ID for current user
                var clientId = await GetClientIdForCurrentUserAsync();
                
                // Record proposal viewed using domain service
                await _clientPortalService.RecordProposalViewedAsync(proposalId, clientId);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ProposalNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in RecordProposalViewedAsync for proposal {ProposalId}", proposalId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        public virtual async Task<ClientPortalProposalDto> RespondToProposalAsync(Guid proposalId, ProposalResponseDto input)
        {
            try
            {
                // Get client ID for current user
                var clientId = await GetClientIdForCurrentUserAsync();
                
                // Record proposal response using domain service
                await _clientPortalService.RecordProposalResponseAsync(
                    proposalId,
                    clientId,
                    input.Accepted);
                
                // Get updated proposal
                var proposal = await _proposalRepository.GetAsync(proposalId);
                
                // Convert to DTO
                var proposalDto = new ClientPortalProposalDto
                {
                    Id = proposal.Id,
                    Title = proposal.Title,
                    Status = proposal.Status,
                    SentDate = proposal.SentDate,
                    ExpirationDate = proposal.ExpirationDate,
                    IsExpired = proposal.IsExpired(),
                    ProjectId = proposal.ProjectId
                };
                
                // Get project name if project ID is provided
                if (proposal.ProjectId.HasValue)
                {
                    try
                    {
                        var project = await _projectRepository.GetAsync(proposal.ProjectId.Value);
                        proposalDto.ProjectName = project.Name;
                    }
                    catch
                    {
                        proposalDto.ProjectName = "Unknown Project";
                    }
                }
                
                // Get document ID if available
                if (proposal.CurrentVersionId.HasValue)
                {
                    var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                    if (currentVersion != null)
                    {
                        proposalDto.DocumentId = currentVersion.DocumentId.Value;
                    }
                }
                
                return proposalDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ProposalNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in RespondToProposalAsync for proposal {ProposalId}", proposalId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        /// <summary>
        /// Helper method to get client ID associated with current user
        /// </summary>
        private async Task<Guid> GetClientIdForCurrentUserAsync()
        {
            if (!_currentUser.Id.HasValue)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:UserNotAuthenticated"]);
            }
            
            // Try to find client by user ID
            var userId = _currentUser.Id.Value;
            var clientsByEmail = await _clientRepository.GetListAsync(
                c => c.ContactInfo.Email == _currentUser.Email);
            
            if (clientsByEmail.Count == 0)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ClientNotFound"]);
            }
            
            // Return the first client found with matching email
            return clientsByEmail.First().Id;
        }
    }
}
