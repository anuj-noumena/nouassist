using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services;
using NouAssist.BusinessOperations.FreelanceOperations.Permissions;
using NouAssist.BusinessOperations.FreelanceOperations.Proposals.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace NouAssist.BusinessOperations.FreelanceOperations.Proposals
{
    [Authorize(FreelanceOperationsPermissions.Proposal.Default)]
    public class ProposalAppService : ApplicationService, IProposalAppService
    {
        private readonly IProposalRepository _proposalRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IFreelanceProjectRepository _projectRepository;
        private readonly IProposalManager _proposalManager;
        private readonly ICurrentUser _currentUser;
        
        public ProposalAppService(
            IProposalRepository proposalRepository,
            IClientRepository clientRepository,
            IFreelanceProjectRepository projectRepository,
            IProposalManager proposalManager,
            ICurrentUser currentUser)
        {
            _proposalRepository = proposalRepository;
            _clientRepository = clientRepository;
            _projectRepository = projectRepository;
            _proposalManager = proposalManager;
            _currentUser = currentUser;
            
            LocalizationResource = typeof(FreelanceOperationsResource);
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.View)]
        public virtual async Task<ProposalDto> GetAsync(Guid id)
        {
            try
            {
                // Get proposal with details
                var proposal = await _proposalRepository.GetAsync(id);
                
                // Map to DTO
                var proposalDto = ObjectMapper.Map<Proposal, ProposalDto>(proposal);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(proposal.ClientId);
                    proposalDto.ClientName = client.Name;
                }
                catch
                {
                    proposalDto.ClientName = "Unknown Client";
                }
                
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
                
                // Get current version if available
                if (proposal.CurrentVersionId.HasValue)
                {
                    var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                    if (currentVersion != null)
                    {
                        var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(currentVersion);
                        
                        // Set created by name if possible
                        try
                        {
                            versionDto.CreatedByName = _currentUser.UserName;
                        }
                        catch
                        {
                            versionDto.CreatedByName = "Unknown User";
                        }
                        
                        proposalDto.CurrentVersion = versionDto;
                    }
                }
                
                return proposalDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ProposalNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetAsync for proposal {ProposalId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.View)]
        public virtual async Task<PagedResultDto<ProposalDto>> GetListAsync(GetProposalListDto input)
        {
            try
            {
                // Prepare query
                var query = await _proposalRepository.GetQueryableAsync();
                
                // Apply filters
                if (!string.IsNullOrEmpty(input.Filter))
                {
                    query = query.Where(p => p.Title.Contains(input.Filter));
                }
                
                if (input.ClientId.HasValue)
                {
                    query = query.Where(p => p.ClientId == input.ClientId.Value);
                }
                
                if (input.ProjectId.HasValue)
                {
                    query = query.Where(p => p.ProjectId == input.ProjectId.Value);
                }
                
                if (input.Status.HasValue)
                {
                    query = query.Where(p => p.Status == input.Status.Value);
                }
                
                if (input.SentDateMin.HasValue)
                {
                    query = query.Where(p => p.SentDate >= input.SentDateMin.Value);
                }
                
                if (input.SentDateMax.HasValue)
                {
                    query = query.Where(p => p.SentDate <= input.SentDateMax.Value);
                }
                
                // Handle special filter for expired proposals
                if (input.IsExpired.HasValue)
                {
                    if (input.IsExpired.Value)
                    {
                        query = query.Where(p => p.ExpirationDate.HasValue && p.ExpirationDate.Value < DateTime.UtcNow);
                    }
                    else
                    {
                        query = query.Where(p => !p.ExpirationDate.HasValue || p.ExpirationDate.Value >= DateTime.UtcNow);
                    }
                }
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting
                query = query.OrderBy(input.Sorting ?? nameof(Proposal.CreationTime) + " desc");
                
                // Apply paging
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
                
                // Execute query
                var proposals = await AsyncExecuter.ToListAsync(query);
                
                // Map to DTOs
                var proposalDtos = ObjectMapper.Map<List<Proposal>, List<ProposalDto>>(proposals);
                
                // Get client names
                var clientIds = proposalDtos.Select(p => p.ClientId).Distinct().ToList();
                var clients = await _clientRepository.GetListAsync(c => clientIds.Contains(c.Id));
                
                // Get project names
                var projectIds = proposalDtos
                    .Where(p => p.ProjectId.HasValue)
                    .Select(p => p.ProjectId.Value)
                    .Distinct()
                    .ToList();
                
                var projects = await _projectRepository.GetListAsync(p => projectIds.Contains(p.Id));
                
                foreach (var proposalDto in proposalDtos)
                {
                    // Set client name
                    var client = clients.FirstOrDefault(c => c.Id == proposalDto.ClientId);
                    proposalDto.ClientName = client?.Name ?? "Unknown Client";
                    
                    // Set project name if applicable
                    if (proposalDto.ProjectId.HasValue)
                    {
                        var project = projects.FirstOrDefault(p => p.Id == proposalDto.ProjectId.Value);
                        proposalDto.ProjectName = project?.Name ?? "Unknown Project";
                    }
                    
                    // Get current version if available (for summaries only)
                    var proposal = proposals.First(p => p.Id == proposalDto.Id);
                    if (proposal.CurrentVersionId.HasValue)
                    {
                        var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                        if (currentVersion != null)
                        {
                            var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(currentVersion);
                            
                            // Set created by name if possible
                            try
                            {
                                versionDto.CreatedByName = _currentUser.UserName;
                            }
                            catch
                            {
                                versionDto.CreatedByName = "Unknown User";
                            }
                            
                            proposalDto.CurrentVersion = versionDto;
                        }
                    }
                }
                
                return new PagedResultDto<ProposalDto>(totalCount, proposalDtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetListAsync for proposals");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.Create)]
        public virtual async Task<ProposalDto> CreateAsync(CreateProposalDto input)
        {
            try
            {
                // Create template reference value object
                var templateReference = ObjectMapper.Map<CreateProposalDto, ProposalTemplateReference>(input);
                
                // Create proposal using domain service
                var proposal = await _proposalManager.CreateProposalAsync(
                    input.Title,
                    input.ClientId,
                    templateReference,
                    input.ProjectId,
                    input.ExpirationDate);
                
                // Map to DTO
                var proposalDto = ObjectMapper.Map<Proposal, ProposalDto>(proposal);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(input.ClientId);
                    proposalDto.ClientName = client.Name;
                }
                catch
                {
                    proposalDto.ClientName = "Unknown Client";
                }
                
                // Get project name if project ID is provided
                if (input.ProjectId.HasValue)
                {
                    try
                    {
                        var project = await _projectRepository.GetAsync(input.ProjectId.Value);
                        proposalDto.ProjectName = project.Name;
                    }
                    catch
                    {
                        proposalDto.ProjectName = "Unknown Project";
                    }
                }
                
                return proposalDto;
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in CreateAsync for proposal");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.Update)]
        public virtual async Task<ProposalDto> UpdateAsync(Guid id, UpdateProposalDto input)
        {
            try
            {
                // Get proposal to update
                var proposal = await _proposalRepository.GetAsync(id);
                
                // Create template reference value object
                var templateReference = ObjectMapper.Map<UpdateProposalDto, ProposalTemplateReference>(input);
                
                // Update basic info
                proposal.UpdateBasicInfo(
                    input.Title,
                    templateReference,
                    input.ExpirationDate);
                
                // Save changes
                await _proposalRepository.UpdateAsync(proposal);
                
                // Map to DTO
                var proposalDto = ObjectMapper.Map<Proposal, ProposalDto>(proposal);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(proposal.ClientId);
                    proposalDto.ClientName = client.Name;
                }
                catch
                {
                    proposalDto.ClientName = "Unknown Client";
                }
                
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
                
                // Get current version if available
                if (proposal.CurrentVersionId.HasValue)
                {
                    var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                    if (currentVersion != null)
                    {
                        var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(currentVersion);
                        
                        // Set created by name if possible
                        try
                        {
                            versionDto.CreatedByName = _currentUser.UserName;
                        }
                        catch
                        {
                            versionDto.CreatedByName = "Unknown User";
                        }
                        
                        proposalDto.CurrentVersion = versionDto;
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
                Logger.LogError(ex, "Error in UpdateAsync for proposal {ProposalId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _proposalRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException)
            {
                // Do nothing if entity not found
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in DeleteAsync for proposal {ProposalId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        #region Version Management

        [Authorize(FreelanceOperationsPermissions.Proposal.View)]
        public virtual async Task<ProposalVersionDto> GetVersionAsync(Guid id)
        {
            try
            {
                // Get all proposals
                var proposals = await _proposalRepository.GetListAsync();
                
                // Find the version
                foreach (var proposal in proposals)
                {
                    var version = proposal.Versions.FirstOrDefault(v => v.Id == id);
                    if (version != null)
                    {
                        // Map to DTO
                        var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(version);
                        
                        // Set created by name if possible
                        try
                        {
                            versionDto.CreatedByName = _currentUser.UserName;
                        }
                        catch
                        {
                            versionDto.CreatedByName = "Unknown User";
                        }
                        
                        return versionDto;
                    }
                }
                
                throw new EntityNotFoundException(typeof(ProposalVersion), id);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ProposalVersionNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetVersionAsync for version {VersionId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.View)]
        public virtual async Task<List<ProposalVersionDto>> GetVersionsAsync(Guid proposalId)
        {
            try
            {
                // Get proposal with versions
                var proposal = await _proposalRepository.GetAsync(proposalId);
                
                // Map to DTOs
                var versionDtos = ObjectMapper.Map<List<ProposalVersion>, List<ProposalVersionDto>>(proposal.Versions.ToList());
                
                // Set created by name if possible
                foreach (var versionDto in versionDtos)
                {
                    try
                    {
                        versionDto.CreatedByName = _currentUser.UserName;
                    }
                    catch
                    {
                        versionDto.CreatedByName = "Unknown User";
                    }
                }
                
                return versionDtos;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ProposalNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetVersionsAsync for proposal {ProposalId}", proposalId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.ManageVersions)]
        public virtual async Task<ProposalVersionDto> AddVersionAsync(Guid proposalId, CreateProposalVersionDto input)
        {
            try
            {
                // Create content summary value object
                var contentSummary = ObjectMapper.Map<CreateProposalVersionDto, ProposalContentSummary>(input);
                
                // Create pricing structure value object
                var pricing = ObjectMapper.Map<CreateProposalVersionDto, PricingStructure>(input);
                if (pricing == null)
                {
                    throw new BusinessException(L["FreelanceOperations:Validation:InvalidPricingStructure"]);
                }
                
                // Create document ID value object
                var documentId = ObjectMapper.Map<CreateProposalVersionDto, DocumentId>(input);
                
                // Add version using domain service
                var version = await _proposalManager.AddProposalVersionAsync(
                    proposalId,
                    input.VersionNumber,
                    input.VersionName,
                    contentSummary,
                    pricing,
                    documentId,
                    input.Notes);
                
                // Map to DTO
                var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(version);
                
                // Set created by name if possible
                try
                {
                    versionDto.CreatedByName = _currentUser.UserName;
                }
                catch
                {
                    versionDto.CreatedByName = "Unknown User";
                }
                
                return versionDto;
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
                Logger.LogError(ex, "Error in AddVersionAsync for proposal {ProposalId}", proposalId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.ManageVersions)]
        public virtual async Task<ProposalVersionDto> SetCurrentVersionAsync(Guid proposalId, Guid versionId)
        {
            try
            {
                // Get proposal
                var proposal = await _proposalRepository.GetAsync(proposalId);
                
                // Set current version
                proposal.SetCurrentVersion(versionId);
                
                // Save changes
                await _proposalRepository.UpdateAsync(proposal);
                
                // Get the version
                var version = proposal.Versions.First(v => v.Id == versionId);
                
                // Map to DTO
                var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(version);
                
                // Set created by name if possible
                try
                {
                    versionDto.CreatedByName = _currentUser.UserName;
                }
                catch
                {
                    versionDto.CreatedByName = "Unknown User";
                }
                
                return versionDto;
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
                Logger.LogError(ex, "Error in SetCurrentVersionAsync for proposal {ProposalId}, version {VersionId}", 
                    proposalId, versionId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }
        #endregion

        #region Status Operations

        [Authorize(FreelanceOperationsPermissions.Proposal.Send)]
        public virtual async Task<ProposalDto> SendAsync(Guid id, SendProposalDto input)
        {
            try
            {
                // Send proposal using domain service
                await _proposalManager.SendProposalAsync(id, input.SentToEmail);
                
                // Get updated proposal
                var proposal = await _proposalRepository.GetAsync(id);
                
                // Map to DTO
                var proposalDto = ObjectMapper.Map<Proposal, ProposalDto>(proposal);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(proposal.ClientId);
                    proposalDto.ClientName = client.Name;
                }
                catch
                {
                    proposalDto.ClientName = "Unknown Client";
                }
                
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
                
                // Get current version if available
                if (proposal.CurrentVersionId.HasValue)
                {
                    var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                    if (currentVersion != null)
                    {
                        var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(currentVersion);
                        
                        // Set created by name if possible
                        try
                        {
                            versionDto.CreatedByName = _currentUser.UserName;
                        }
                        catch
                        {
                            versionDto.CreatedByName = "Unknown User";
                        }
                        
                        proposalDto.CurrentVersion = versionDto;
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
                Logger.LogError(ex, "Error in SendAsync for proposal {ProposalId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.Update)]
        public virtual async Task<ProposalDto> MarkAsViewedAsync(Guid id)
        {
            try
            {
                // Get proposal
                var proposal = await _proposalRepository.GetAsync(id);
                
                // Mark as viewed
                proposal.MarkAsViewed(DateTime.UtcNow);
                
                // Save changes
                await _proposalRepository.UpdateAsync(proposal);
                
                // Map to DTO
                var proposalDto = ObjectMapper.Map<Proposal, ProposalDto>(proposal);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(proposal.ClientId);
                    proposalDto.ClientName = client.Name;
                }
                catch
                {
                    proposalDto.ClientName = "Unknown Client";
                }
                
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
                
                // Get current version if available
                if (proposal.CurrentVersionId.HasValue)
                {
                    var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                    if (currentVersion != null)
                    {
                        var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(currentVersion);
                        
                        // Set created by name if possible
                        try
                        {
                            versionDto.CreatedByName = _currentUser.UserName;
                        }
                        catch
                        {
                            versionDto.CreatedByName = "Unknown User";
                        }
                        
                        proposalDto.CurrentVersion = versionDto;
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
                Logger.LogError(ex, "Error in MarkAsViewedAsync for proposal {ProposalId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.Update)]
        public virtual async Task<ProposalDto> AcceptAsync(Guid id)
        {
            try
            {
                // Accept proposal using domain service
                await _proposalManager.AcceptProposalAsync(id);
                
                // Get updated proposal
                var proposal = await _proposalRepository.GetAsync(id);
                
                // Map to DTO
                var proposalDto = ObjectMapper.Map<Proposal, ProposalDto>(proposal);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(proposal.ClientId);
                    proposalDto.ClientName = client.Name;
                }
                catch
                {
                    proposalDto.ClientName = "Unknown Client";
                }
                
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
                
                // Get current version if available
                if (proposal.CurrentVersionId.HasValue)
                {
                    var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                    if (currentVersion != null)
                    {
                        var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(currentVersion);
                        
                        // Set created by name if possible
                        try
                        {
                            versionDto.CreatedByName = _currentUser.UserName;
                        }
                        catch
                        {
                            versionDto.CreatedByName = "Unknown User";
                        }
                        
                        proposalDto.CurrentVersion = versionDto;
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
                Logger.LogError(ex, "Error in AcceptAsync for proposal {ProposalId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.Update)]
        public virtual async Task<ProposalDto> RejectAsync(Guid id)
        {
            try
            {
                // Reject proposal using domain service
                await _proposalManager.RejectProposalAsync(id);
                
                // Get updated proposal
                var proposal = await _proposalRepository.GetAsync(id);
                
                // Map to DTO
                var proposalDto = ObjectMapper.Map<Proposal, ProposalDto>(proposal);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(proposal.ClientId);
                    proposalDto.ClientName = client.Name;
                }
                catch
                {
                    proposalDto.ClientName = "Unknown Client";
                }
                
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
                
                // Get current version if available
                if (proposal.CurrentVersionId.HasValue)
                {
                    var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                    if (currentVersion != null)
                    {
                        var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(currentVersion);
                        
                        // Set created by name if possible
                        try
                        {
                            versionDto.CreatedByName = _currentUser.UserName;
                        }
                        catch
                        {
                            versionDto.CreatedByName = "Unknown User";
                        }
                        
                        proposalDto.CurrentVersion = versionDto;
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
                Logger.LogError(ex, "Error in RejectAsync for proposal {ProposalId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.Update)]
        public virtual async Task<ProposalDto> MarkAsNeedsRevisionAsync(Guid id)
        {
            try
            {
                // Mark proposal as needing revision using domain service
                await _proposalManager.MarkProposalAsNeedsRevisionAsync(id);
                
                // Get updated proposal
                var proposal = await _proposalRepository.GetAsync(id);
                
                // Map to DTO
                var proposalDto = ObjectMapper.Map<Proposal, ProposalDto>(proposal);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(proposal.ClientId);
                    proposalDto.ClientName = client.Name;
                }
                catch
                {
                    proposalDto.ClientName = "Unknown Client";
                }
                
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
                
                // Get current version if available
                if (proposal.CurrentVersionId.HasValue)
                {
                    var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                    if (currentVersion != null)
                    {
                        var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(currentVersion);
                        
                        // Set created by name if possible
                        try
                        {
                            versionDto.CreatedByName = _currentUser.UserName;
                        }
                        catch
                        {
                            versionDto.CreatedByName = "Unknown User";
                        }
                        
                        proposalDto.CurrentVersion = versionDto;
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
                Logger.LogError(ex, "Error in MarkAsNeedsRevisionAsync for proposal {ProposalId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.Update)]
        public virtual async Task<ProposalDto> MarkAsExpiredAsync(Guid id)
        {
            try
            {
                // Get proposal
                var proposal = await _proposalRepository.GetAsync(id);
                
                // Mark as expired
                proposal.MarkAsExpired(DateTime.UtcNow);
                
                // Save changes
                await _proposalRepository.UpdateAsync(proposal);
                
                // Map to DTO
                var proposalDto = ObjectMapper.Map<Proposal, ProposalDto>(proposal);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(proposal.ClientId);
                    proposalDto.ClientName = client.Name;
                }
                catch
                {
                    proposalDto.ClientName = "Unknown Client";
                }
                
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
                
                // Get current version if available
                if (proposal.CurrentVersionId.HasValue)
                {
                    var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                    if (currentVersion != null)
                    {
                        var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(currentVersion);
                        
                        // Set created by name if possible
                        try
                        {
                            versionDto.CreatedByName = _currentUser.UserName;
                        }
                        catch
                        {
                            versionDto.CreatedByName = "Unknown User";
                        }
                        
                        proposalDto.CurrentVersion = versionDto;
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
                Logger.LogError(ex, "Error in MarkAsExpiredAsync for proposal {ProposalId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }
        #endregion

        #region Query Methods

        [Authorize(FreelanceOperationsPermissions.Proposal.View)]
        public virtual async Task<List<ProposalDto>> GetProposalsByClientAsync(Guid clientId)
        {
            try
            {
                // Get client to verify it exists
                await _clientRepository.GetAsync(clientId);
                
                // Get proposals for this client
                var proposals = await _proposalRepository.GetProposalsByClientAsync(clientId);
                
                // Map to DTOs
                var proposalDtos = ObjectMapper.Map<List<Proposal>, List<ProposalDto>>(proposals);
                
                // Get client name
                var client = await _clientRepository.GetAsync(clientId);
                
                // Get project names
                var projectIds = proposalDtos
                    .Where(p => p.ProjectId.HasValue)
                    .Select(p => p.ProjectId.Value)
                    .Distinct()
                    .ToList();
                
                var projects = await _projectRepository.GetListAsync(p => projectIds.Contains(p.Id));
                
                foreach (var proposalDto in proposalDtos)
                {
                    // Set client name
                    proposalDto.ClientName = client.Name;
                    
                    // Set project name if applicable
                    if (proposalDto.ProjectId.HasValue)
                    {
                        var project = projects.FirstOrDefault(p => p.Id == proposalDto.ProjectId.Value);
                        proposalDto.ProjectName = project?.Name ?? "Unknown Project";
                    }
                    
                    // Get current version if available (only for essential summary info)
                    var proposal = proposals.FirstOrDefault(p => p.Id == proposalDto.Id);
                    if (proposal.CurrentVersionId.HasValue)
                    {
                        var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                        if (currentVersion != null)
                        {
                            var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(currentVersion);
                            
                            // Set created by name if possible
                            try
                            {
                                versionDto.CreatedByName = _currentUser.UserName;
                            }
                            catch
                            {
                                versionDto.CreatedByName = "Unknown User";
                            }
                            
                            proposalDto.CurrentVersion = versionDto;
                        }
                    }
                }
                
                return proposalDtos;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ClientNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetProposalsByClientAsync for client {ClientId}", clientId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.View)]
        public virtual async Task<List<ProposalDto>> GetProposalsByProjectAsync(Guid projectId)
        {
            try
            {
                // Get project to verify it exists
                await _projectRepository.GetAsync(projectId);
                
                // Get proposals for this project
                var proposals = await _proposalRepository.GetProposalsByProjectAsync(projectId);
                
                // Map to DTOs
                var proposalDtos = ObjectMapper.Map<List<Proposal>, List<ProposalDto>>(proposals);
                
                // Get client names
                var clientIds = proposalDtos.Select(p => p.ClientId).Distinct().ToList();
                var clients = await _clientRepository.GetListAsync(c => clientIds.Contains(c.Id));
                
                // Get project
                var project = await _projectRepository.GetAsync(projectId);
                
                foreach (var proposalDto in proposalDtos)
                {
                    // Set client name
                    var client = clients.FirstOrDefault(c => c.Id == proposalDto.ClientId);
                    proposalDto.ClientName = client?.Name ?? "Unknown Client";
                    
                    // Set project name
                    proposalDto.ProjectName = project.Name;
                    
                    // Get current version if available (only for essential summary info)
                    var proposal = proposals.FirstOrDefault(p => p.Id == proposalDto.Id);
                    if (proposal.CurrentVersionId.HasValue)
                    {
                        var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                        if (currentVersion != null)
                        {
                            var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(currentVersion);
                            
                            // Set created by name if possible
                            try
                            {
                                versionDto.CreatedByName = _currentUser.UserName;
                            }
                            catch
                            {
                                versionDto.CreatedByName = "Unknown User";
                            }
                            
                            proposalDto.CurrentVersion = versionDto;
                        }
                    }
                }
                
                return proposalDtos;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ProjectNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetProposalsByProjectAsync for project {ProjectId}", projectId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Proposal.View)]
        public virtual async Task<List<ProposalDto>> GetProposalsNearingExpirationAsync(int daysUntilExpiration = 3)
        {
            try
            {
                // Get proposals nearing expiration using domain service
                var proposals = await _proposalManager.GetProposalsNearingExpirationAsync(daysUntilExpiration);
                
                // Map to DTOs
                var proposalDtos = ObjectMapper.Map<List<Proposal>, List<ProposalDto>>(proposals);
                
                // Get client names
                var clientIds = proposalDtos.Select(p => p.ClientId).Distinct().ToList();
                var clients = await _clientRepository.GetListAsync(c => clientIds.Contains(c.Id));
                
                // Get project names
                var projectIds = proposalDtos
                    .Where(p => p.ProjectId.HasValue)
                    .Select(p => p.ProjectId.Value)
                    .Distinct()
                    .ToList();
                
                var projects = await _projectRepository.GetListAsync(p => projectIds.Contains(p.Id));
                
                foreach (var proposalDto in proposalDtos)
                {
                    // Set client name
                    var client = clients.FirstOrDefault(c => c.Id == proposalDto.ClientId);
                    proposalDto.ClientName = client?.Name ?? "Unknown Client";
                    
                    // Set project name if applicable
                    if (proposalDto.ProjectId.HasValue)
                    {
                        var project = projects.FirstOrDefault(p => p.Id == proposalDto.ProjectId.Value);
                        proposalDto.ProjectName = project?.Name ?? "Unknown Project";
                    }
                    
                    // Get current version if available (only for essential summary info)
                    var proposal = proposals.FirstOrDefault(p => p.Id == proposalDto.Id);
                    if (proposal.CurrentVersionId.HasValue)
                    {
                        var currentVersion = proposal.Versions.FirstOrDefault(v => v.Id == proposal.CurrentVersionId.Value);
                        if (currentVersion != null)
                        {
                            var versionDto = ObjectMapper.Map<ProposalVersion, ProposalVersionDto>(currentVersion);
                            
                            // Set created by name if possible
                            try
                            {
                                versionDto.CreatedByName = _currentUser.UserName;
                            }
                            catch
                            {
                                versionDto.CreatedByName = "Unknown User";
                            }
                            
                            proposalDto.CurrentVersion = versionDto;
                        }
                    }
                }
                
                return proposalDtos;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetProposalsNearingExpirationAsync");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }
        #endregion
    }
}
