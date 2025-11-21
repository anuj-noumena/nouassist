using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services;
using NouAssist.BusinessOperations.FreelanceOperations.Localization;
using NouAssist.BusinessOperations.FreelanceOperations.Permissions;
using NouAssist.BusinessOperations.FreelanceOperations.Specifications;
using NouAssist.BusinessOperations.FreelanceOperations.Upwork.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.FreelanceOperations.Upwork
{
    [Authorize(FreelanceOperationsPermissions.Upwork.Default)]
    public class UpworkAppService : ApplicationService, IUpworkAppService
    {
        private readonly IUpworkEngagementRepository _engagementRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IFreelanceProjectRepository _projectRepository;
        private readonly IUpworkConnector _upworkConnector;
        
        public UpworkAppService(
            IUpworkEngagementRepository engagementRepository,
            IClientRepository clientRepository,
            IFreelanceProjectRepository projectRepository,
            IUpworkConnector upworkConnector)
        {
            _engagementRepository = engagementRepository;
            _clientRepository = clientRepository;
            _projectRepository = projectRepository;
            _upworkConnector = upworkConnector;
            
            LocalizationResource = typeof(FreelanceOperationsResource);
        }

        #region Credentials Management
        [Authorize(FreelanceOperationsPermissions.Upwork.ManageCredentials)]
        public virtual async Task<UpworkCredentialsDto> GetCredentialsAsync()
        {
            try
            {
                var credentials = await _upworkConnector.GetCredentialsAsync();
                
                if (credentials == null)
                {
                    return new UpworkCredentialsDto
                    {
                        ApiKey = "",
                        ApiSecret = "",
                        IsTokenExpired = true
                    };
                }
                
                return ObjectMapper.Map<UpworkCredentials, UpworkCredentialsDto>(credentials);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetCredentialsAsync");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Upwork.ManageCredentials)]
        public virtual async Task SetCredentialsAsync(UpworkCredentialsDto input)
        {
            try
            {
                var credentials = ObjectMapper.Map<UpworkCredentialsDto, UpworkCredentials>(input);
                await _upworkConnector.SetCredentialsAsync(credentials);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in SetCredentialsAsync");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }
        #endregion

        #region Engagement Management
        [Authorize(FreelanceOperationsPermissions.Upwork.View)]
        public virtual async Task<UpworkEngagementDto> GetEngagementAsync(Guid id)
        {
            try
            {
                var engagement = await _engagementRepository.GetAsync(id);
                
                // Map to DTO
                var engagementDto = ObjectMapper.Map<UpworkEngagement, UpworkEngagementDto>(engagement);
                
                // Get linked project name if applicable
                if (engagement.LinkedProjectId.HasValue)
                {
                    try
                    {
                        var project = await _projectRepository.GetAsync(engagement.LinkedProjectId.Value);
                        engagementDto.LinkedProjectName = project.Name;
                    }
                    catch
                    {
                        engagementDto.LinkedProjectName = "Unknown Project";
                    }
                }
                
                // Get linked client name if applicable
                if (engagement.LinkedClientId.HasValue)
                {
                    try
                    {
                        var client = await _clientRepository.GetAsync(engagement.LinkedClientId.Value);
                        engagementDto.LinkedClientName = client.Name;
                    }
                    catch
                    {
                        engagementDto.LinkedClientName = "Unknown Client";
                    }
                }
                
                return engagementDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:UpworkEngagementNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetEngagementAsync for engagement {EngagementId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Upwork.View)]
        public virtual async Task<PagedResultDto<UpworkEngagementDto>> GetListAsync(GetUpworkEngagementListDto input)
        {
            try
            {
                // Prepare query
                var query = await _engagementRepository.GetQueryableAsync();
                
                // Apply filters
                if (!string.IsNullOrEmpty(input.Filter))
                {
                    query = query.Where(e => 
                        e.JobTitle.Contains(input.Filter) || 
                        e.ClientName.Contains(input.Filter) || 
                        e.UpworkJobId.Contains(input.Filter) || 
                        e.UpworkContractId.Contains(input.Filter));
                }
                
                if (!string.IsNullOrEmpty(input.Status))
                {
                    query = query.Where(e => e.Status == input.Status);
                }
                
                if (input.IsLinkedToSystem.HasValue)
                {
                    if (input.IsLinkedToSystem.Value)
                    {
                        query = query.Where(e => e.LinkedProjectId.HasValue || e.LinkedClientId.HasValue);
                    }
                    else
                    {
                        query = query.Where(e => !e.LinkedProjectId.HasValue && !e.LinkedClientId.HasValue);
                    }
                }
                
                if (input.StartDateMin.HasValue)
                {
                    query = query.Where(e => e.StartDate >= input.StartDateMin.Value);
                }
                
                if (input.StartDateMax.HasValue)
                {
                    query = query.Where(e => e.StartDate <= input.StartDateMax.Value);
                }
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting
                query = query.OrderBy(input.Sorting ?? nameof(UpworkEngagement.CreationTime) + " desc");
                
                // Apply paging
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
                
                // Execute query
                var engagements = await AsyncExecuter.ToListAsync(query);
                
                // Map to DTOs
                var engagementDtos = ObjectMapper.Map<List<UpworkEngagement>, List<UpworkEngagementDto>>(engagements);
                
                // Get linked project and client names
                var projectIds = engagementDtos
                    .Where(e => e.LinkedProjectId.HasValue)
                    .Select(e => e.LinkedProjectId.Value)
                    .Distinct()
                    .ToList();
                
                var clientIds = engagementDtos
                    .Where(e => e.LinkedClientId.HasValue)
                    .Select(e => e.LinkedClientId.Value)
                    .Distinct()
                    .ToList();
                
                var projects = await _projectRepository.GetListAsync(p => projectIds.Contains(p.Id));
                var clients = await _clientRepository.GetListAsync(c => clientIds.Contains(c.Id));
                
                foreach (var engagementDto in engagementDtos)
                {
                    // Set linked project name if applicable
                    if (engagementDto.LinkedProjectId.HasValue)
                    {
                        var project = projects.FirstOrDefault(p => p.Id == engagementDto.LinkedProjectId.Value);
                        engagementDto.LinkedProjectName = project?.Name ?? "Unknown Project";
                    }
                    
                    // Set linked client name if applicable
                    if (engagementDto.LinkedClientId.HasValue)
                    {
                        var client = clients.FirstOrDefault(c => c.Id == engagementDto.LinkedClientId.Value);
                        engagementDto.LinkedClientName = client?.Name ?? "Unknown Client";
                    }
                }
                
                return new PagedResultDto<UpworkEngagementDto>(totalCount, engagementDtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetListAsync for Upwork engagements");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Upwork.ImportJobs)]
        public virtual async Task<UpworkEngagementDto> ImportJobAsync(ImportUpworkJobDto input)
        {
            try
            {
                // Import job using domain service
                var engagement = await _upworkConnector.ImportJobAsync(
                    input.UpworkJobId,
                    input.JobTitle,
                    input.ClientName,
                    input.JobLink,
                    input.ContractType);
                
                // Map to DTO
                var engagementDto = ObjectMapper.Map<UpworkEngagement, UpworkEngagementDto>(engagement);
                
                return engagementDto;
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in ImportJobAsync for job {JobId}", input.UpworkJobId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Upwork.ManageEngagements)]
        public virtual async Task<UpworkEngagementDto> UpdateContractAsync(Guid id, UpdateUpworkContractDto input)
        {
            try
            {
                // Update contract using domain service
                await _upworkConnector.UpdateEngagementWithContractAsync(
                    id,
                    input.UpworkContractId,
                    input.StartDate);
                
                // Get updated engagement
                var engagement = await _engagementRepository.GetAsync(id);
                
                // Map to DTO
                var engagementDto = ObjectMapper.Map<UpworkEngagement, UpworkEngagementDto>(engagement);
                
                return engagementDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:UpworkEngagementNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in UpdateContractAsync for engagement {EngagementId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Upwork.ManageEngagements)]
        public virtual async Task<UpworkEngagementDto> LinkToSystemAsync(Guid id, LinkEngagementToSystemDto input)
        {
            try
            {
                // Link to system using domain service
                await _upworkConnector.LinkEngagementToSystemAsync(
                    id,
                    input.ProjectId,
                    input.ClientId);
                
                // Get updated engagement
                var engagement = await _engagementRepository.GetAsync(id);
                
                // Map to DTO
                var engagementDto = ObjectMapper.Map<UpworkEngagement, UpworkEngagementDto>(engagement);
                
                // Get linked project name if applicable
                if (input.ProjectId.HasValue)
                {
                    try
                    {
                        var project = await _projectRepository.GetAsync(input.ProjectId.Value);
                        engagementDto.LinkedProjectName = project.Name;
                    }
                    catch
                    {
                        engagementDto.LinkedProjectName = "Unknown Project";
                    }
                }
                
                // Get linked client name if applicable
                if (input.ClientId.HasValue)
                {
                    try
                    {
                        var client = await _clientRepository.GetAsync(input.ClientId.Value);
                        engagementDto.LinkedClientName = client.Name;
                    }
                    catch
                    {
                        engagementDto.LinkedClientName = "Unknown Client";
                    }
                }
                
                return engagementDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:UpworkEngagementNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in LinkToSystemAsync for engagement {EngagementId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Upwork.ManageEngagements)]
        public virtual async Task<UpworkEngagementDto> UpdateStatusAsync(Guid id, string status)
        {
            try
            {
                // Get engagement
                var engagement = await _engagementRepository.GetAsync(id);
                
                // Update status
                engagement.UpdateStatus(status);
                
                // Save changes
                await _engagementRepository.UpdateAsync(engagement);
                
                // Map to DTO
                var engagementDto = ObjectMapper.Map<UpworkEngagement, UpworkEngagementDto>(engagement);
                
                // Get linked project name if applicable
                if (engagement.LinkedProjectId.HasValue)
                {
                    try
                    {
                        var project = await _projectRepository.GetAsync(engagement.LinkedProjectId.Value);
                        engagementDto.LinkedProjectName = project.Name;
                    }
                    catch
                    {
                        engagementDto.LinkedProjectName = "Unknown Project";
                    }
                }
                
                // Get linked client name if applicable
                if (engagement.LinkedClientId.HasValue)
                {
                    try
                    {
                        var client = await _clientRepository.GetAsync(engagement.LinkedClientId.Value);
                        engagementDto.LinkedClientName = client.Name;
                    }
                    catch
                    {
                        engagementDto.LinkedClientName = "Unknown Client";
                    }
                }
                
                return engagementDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:UpworkEngagementNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in UpdateStatusAsync for engagement {EngagementId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Upwork.ManageEngagements)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _engagementRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException)
            {
                // Do nothing if entity not found
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in DeleteAsync for Upwork engagement {EngagementId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }
        #endregion

        #region Specialized Queries
        [Authorize(FreelanceOperationsPermissions.Upwork.View)]
        public virtual async Task<List<UpworkEngagementDto>> GetActiveEngagementsAsync()
        {
            try
            {
                // Get active engagements using domain service
                var engagements = await _upworkConnector.GetActiveEngagementsAsync();
                
                // Map to DTOs
                var engagementDtos = ObjectMapper.Map<List<UpworkEngagement>, List<UpworkEngagementDto>>(engagements);
                
                // Get linked project and client names
                var projectIds = engagementDtos
                    .Where(e => e.LinkedProjectId.HasValue)
                    .Select(e => e.LinkedProjectId.Value)
                    .Distinct()
                    .ToList();
                
                var clientIds = engagementDtos
                    .Where(e => e.LinkedClientId.HasValue)
                    .Select(e => e.LinkedClientId.Value)
                    .Distinct()
                    .ToList();
                
                var projects = await _projectRepository.GetListAsync(p => projectIds.Contains(p.Id));
                var clients = await _clientRepository.GetListAsync(c => clientIds.Contains(c.Id));
                
                foreach (var engagementDto in engagementDtos)
                {
                    // Set linked project name if applicable
                    if (engagementDto.LinkedProjectId.HasValue)
                    {
                        var project = projects.FirstOrDefault(p => p.Id == engagementDto.LinkedProjectId.Value);
                        engagementDto.LinkedProjectName = project?.Name ?? "Unknown Project";
                    }
                    
                    // Set linked client name if applicable
                    if (engagementDto.LinkedClientId.HasValue)
                    {
                        var client = clients.FirstOrDefault(c => c.Id == engagementDto.LinkedClientId.Value);
                        engagementDto.LinkedClientName = client?.Name ?? "Unknown Client";
                    }
                }
                
                return engagementDtos;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetActiveEngagementsAsync");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Upwork.View)]
        public virtual async Task<List<UpworkEngagementDto>> GetUnlinkedEngagementsAsync()
        {
            try
            {
                // Use specification to get unlinked engagements
                var spec = new UnlinkedUpworkEngagementsSpecification();
                var engagements = await _engagementRepository.GetListAsync(spec);
                
                // Map to DTOs
                var engagementDtos = ObjectMapper.Map<List<UpworkEngagement>, List<UpworkEngagementDto>>(engagements);
                
                return engagementDtos;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetUnlinkedEngagementsAsync");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }
        #endregion
    }
}
