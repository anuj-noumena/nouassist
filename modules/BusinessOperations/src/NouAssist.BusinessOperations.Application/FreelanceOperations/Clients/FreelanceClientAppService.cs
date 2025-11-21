using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.Clients.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services;
using NouAssist.BusinessOperations.FreelanceOperations.Localization;
using NouAssist.BusinessOperations.FreelanceOperations.Permissions;
using NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace NouAssist.BusinessOperations.FreelanceOperations.Clients
{
    [Authorize(FreelanceOperationsPermissions.Client.Default)]
    public class FreelanceClientAppService : ApplicationService, IFreelanceClientAppService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IFreelanceProjectRepository _projectRepository;
        private readonly IClientManager _clientManager;
        private readonly ICurrentUser _currentUser;
        
        public FreelanceClientAppService(
            IClientRepository clientRepository,
            IFreelanceProjectRepository projectRepository,
            IClientManager clientManager,
            ICurrentUser currentUser)
        {
            _clientRepository = clientRepository;
            _projectRepository = projectRepository;
            _clientManager = clientManager;
            _currentUser = currentUser;
            
            LocalizationResource = typeof(FreelanceOperationsResource);
        }

        [Authorize(FreelanceOperationsPermissions.Client.View)]
        public virtual async Task<FreelanceClientDto> GetAsync(Guid id)
        {
            try
            {
                // Get client with details
                var client = await _clientRepository.GetAsync(id);
                
                // Map to DTO
                var clientDto = ObjectMapper.Map<FreelanceClient, FreelanceClientDto>(client);
                
                // Get project count
                var projectCount = await _projectRepository.CountAsync(p => p.ClientId == id);
                clientDto.ProjectCount = projectCount;
                
                return clientDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ClientNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetAsync for client {ClientId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Client.View)]
        public virtual async Task<PagedResultDto<FreelanceClientDto>> GetListAsync(GetFreelanceClientListDto input)
        {
            try
            {
                // Prepare query
                var query = await _clientRepository.GetQueryableAsync();
                
                // Apply filters
                if (!string.IsNullOrEmpty(input.Filter))
                {
                    query = query.Where(c => 
                        c.Name.Contains(input.Filter) || 
                        c.CompanyName.Contains(input.Filter) || 
                        c.ContactInfo.Email.Contains(input.Filter));
                }
                
                if (input.Status.HasValue)
                {
                    query = query.Where(c => c.Status == input.Status.Value);
                }
                
                if (!string.IsNullOrEmpty(input.Source))
                {
                    query = query.Where(c => c.Source == input.Source);
                }
                
                if (input.HasPendingFollowUps.HasValue && input.HasPendingFollowUps.Value)
                {
                    query = query.Where(c => c.CommunicationRecords.Any(r => r.RequiresFollowUp));
                }
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting
                query = query.OrderBy(input.Sorting ?? nameof(FreelanceClient.Name));
                
                // Apply paging
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
                
                // Execute query
                var clients = await AsyncExecuter.ToListAsync(query);
                
                // Map to DTOs
                var clientDtos = ObjectMapper.Map<List<FreelanceClient>, List<FreelanceClientDto>>(clients);
                
                // Get project counts
                var clientIds = clientDtos.Select(c => c.Id).ToList();
                var projectsByClient = await _projectRepository.GetListAsync(p => clientIds.Contains(p.ClientId));
                
                foreach (var clientDto in clientDtos)
                {
                    clientDto.ProjectCount = projectsByClient.Count(p => p.ClientId == clientDto.Id);
                    
                    // Filter clients with projects if requested
                    if (input.HasProjects.HasValue)
                    {
                        if (input.HasProjects.Value && clientDto.ProjectCount == 0)
                        {
                            totalCount--;
                            continue;
                        }
                        if (!input.HasProjects.Value && clientDto.ProjectCount > 0)
                        {
                            totalCount--;
                            continue;
                        }
                    }
                }
                
                // Filter list if HasProjects filter was applied
                if (input.HasProjects.HasValue)
                {
                    clientDtos = clientDtos.Where(c => 
                        (input.HasProjects.Value && c.ProjectCount > 0) ||
                        (!input.HasProjects.Value && c.ProjectCount == 0)).ToList();
                }
                
                return new PagedResultDto<FreelanceClientDto>(totalCount, clientDtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetListAsync for clients");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Client.Create)]
        public virtual async Task<FreelanceClientDto> CreateAsync(CreateFreelanceClientDto input)
        {
            try
            {
                // Create contact info value object
                var contactInfo = ObjectMapper.Map<CreateFreelanceClientDto, ClientContactInfo>(input);
                
                // Create client using domain service
                var client = await _clientManager.CreateClientAsync(
                    input.Name,
                    contactInfo,
                    input.CompanyName,
                    input.Status,
                    input.Source,
                    input.Notes);
                
                // Map to DTO
                var clientDto = ObjectMapper.Map<FreelanceClient, FreelanceClientDto>(client);
                clientDto.ProjectCount = 0; // New client has no projects
                
                return clientDto;
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in CreateAsync for client");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Client.Update)]
        public virtual async Task<FreelanceClientDto> UpdateAsync(Guid id, UpdateFreelanceClientDto input)
        {
            try
            {
                // Create contact info value object
                var contactInfo = ObjectMapper.Map<UpdateFreelanceClientDto, ClientContactInfo>(input);
                
                // Update client basic info
                await _clientManager.UpdateClientBasicInfoAsync(
                    id,
                    input.Name,
                    input.CompanyName,
                    input.Notes);
                
                // Update client contact info
                await _clientManager.UpdateClientContactInfoAsync(
                    id,
                    contactInfo);
                
                // Get updated client
                var client = await _clientRepository.GetAsync(id);
                
                // Map to DTO
                var clientDto = ObjectMapper.Map<FreelanceClient, FreelanceClientDto>(client);
                
                // Get project count
                var projectCount = await _projectRepository.CountAsync(p => p.ClientId == id);
                clientDto.ProjectCount = projectCount;
                
                return clientDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ClientNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in UpdateAsync for client {ClientId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Client.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _clientRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException)
            {
                // Do nothing if entity not found
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in DeleteAsync for client {ClientId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Client.Update)]
        public virtual async Task<FreelanceClientDto> ChangeStatusAsync(Guid id, ChangeClientStatusDto input)
        {
            try
            {
                // Change status using domain service
                await _clientManager.ChangeClientStatusAsync(id, input.NewStatus);
                
                // Get updated client
                var client = await _clientRepository.GetAsync(id);
                
                // Map to DTO
                var clientDto = ObjectMapper.Map<FreelanceClient, FreelanceClientDto>(client);
                
                // Get project count
                var projectCount = await _projectRepository.CountAsync(p => p.ClientId == id);
                clientDto.ProjectCount = projectCount;
                
                return clientDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ClientNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in ChangeStatusAsync for client {ClientId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Client.View)]
        public virtual async Task<List<FreelanceProjectDto>> GetClientProjectsAsync(Guid id)
        {
            try
            {
                // Get client to verify it exists
                await _clientRepository.GetAsync(id);
                
                // Get projects for this client
                var projects = await _projectRepository.GetProjectsByClientAsync(id);
                
                // Map to DTOs
                var projectDtos = ObjectMapper.Map<List<FreelanceProject>, List<FreelanceProjectDto>>(projects);
                
                // Set client name for all projects
                var client = await _clientRepository.GetAsync(id);
                foreach (var projectDto in projectDtos)
                {
                    projectDto.ClientName = client.Name;
                }
                
                return projectDtos;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ClientNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetClientProjectsAsync for client {ClientId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        #region Communication Records

        [Authorize(FreelanceOperationsPermissions.Client.ManageCommunications)]
        public virtual async Task<ClientCommunicationRecordDto> GetCommunicationRecordAsync(Guid id)
        {
            try
            {
                // Get all clients with communication records
                var clients = await _clientRepository.GetListAsync();
                
                // Find the communication record
                foreach (var client in clients)
                {
                    var record = client.CommunicationRecords.FirstOrDefault(r => r.Id == id);
                    if (record != null)
                    {
                        // Map to DTO
                        var recordDto = ObjectMapper.Map<ClientCommunicationRecord, ClientCommunicationRecordDto>(record);
                        
                        // Get user name if possible
                        try
                        {
                            recordDto.UserName = _currentUser.UserName;
                        }
                        catch
                        {
                            recordDto.UserName = "Unknown User";
                        }
                        
                        // Get project name if project ID is provided
                        if (record.ProjectId.HasValue)
                        {
                            try
                            {
                                var project = await _projectRepository.GetAsync(record.ProjectId.Value);
                                recordDto.ProjectName = project.Name;
                            }
                            catch
                            {
                                recordDto.ProjectName = "Unknown Project";
                            }
                        }
                        
                        return recordDto;
                    }
                }
                
                throw new EntityNotFoundException(typeof(ClientCommunicationRecord), id);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:CommunicationRecordNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetCommunicationRecordAsync for record {RecordId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Client.View)]
        public virtual async Task<List<ClientCommunicationRecordDto>> GetCommunicationRecordsAsync(Guid clientId)
        {
            try
            {
                // Get client with communication records
                var client = await _clientRepository.GetAsync(clientId);
                
                // Map records to DTOs
                var recordDtos = ObjectMapper.Map<List<ClientCommunicationRecord>, List<ClientCommunicationRecordDto>>(
                    client.CommunicationRecords.ToList());
                
                // Get project names for records with project IDs
                var projectIds = recordDtos
                    .Where(r => r.ProjectId.HasValue)
                    .Select(r => r.ProjectId.Value)
                    .Distinct()
                    .ToList();
                
                var projects = await _projectRepository.GetListAsync(p => projectIds.Contains(p.Id));
                
                foreach (var recordDto in recordDtos)
                {
                    // Set user name
                    try
                    {
                        var userName = _currentUser.UserName;
                        recordDto.UserName = userName;
                    }
                    catch
                    {
                        recordDto.UserName = "Unknown User";
                    }
                    
                    // Set project name if applicable
                    if (recordDto.ProjectId.HasValue)
                    {
                        var project = projects.FirstOrDefault(p => p.Id == recordDto.ProjectId.Value);
                        recordDto.ProjectName = project?.Name ?? "Unknown Project";
                    }
                }
                
                return recordDtos;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ClientNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetCommunicationRecordsAsync for client {ClientId}", clientId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Client.ManageCommunications)]
        public virtual async Task<ClientCommunicationRecordDto> AddCommunicationRecordAsync(Guid clientId, CreateCommunicationRecordDto input)
        {
            try
            {
                // Add communication record using domain service
                var record = await _clientManager.AddCommunicationRecordAsync(
                    clientId,
                    input.Type,
                    input.Subject,
                    input.Content,
                    input.ProjectId,
                    input.RequiresFollowUp,
                    input.FollowUpDate);
                
                // Map to DTO
                var recordDto = ObjectMapper.Map<ClientCommunicationRecord, ClientCommunicationRecordDto>(record);
                
                // Set user name
                try
                {
                    var userName = _currentUser.UserName;
                    recordDto.UserName = userName;
                }
                catch
                {
                    recordDto.UserName = "Unknown User";
                }
                
                // Set project name if applicable
                if (input.ProjectId.HasValue)
                {
                    try
                    {
                        var project = await _projectRepository.GetAsync(input.ProjectId.Value);
                        recordDto.ProjectName = project.Name;
                    }
                    catch
                    {
                        recordDto.ProjectName = "Unknown Project";
                    }
                }
                
                return recordDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ClientNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in AddCommunicationRecordAsync for client {ClientId}", clientId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Client.ManageCommunications)]
        public virtual async Task<ClientCommunicationRecordDto> UpdateCommunicationFollowUpAsync(Guid id, UpdateCommunicationFollowUpDto input)
        {
            try
            {
                // Find the client containing this communication record
                var clients = await _clientRepository.GetListAsync();
                ClientCommunicationRecord record = null;
                FreelanceClient client = null;
                
                foreach (var c in clients)
                {
                    var r = c.CommunicationRecords.FirstOrDefault(r => r.Id == id);
                    if (r != null)
                    {
                        record = r;
                        client = c;
                        break;
                    }
                }
                
                if (record == null || client == null)
                {
                    throw new EntityNotFoundException(typeof(ClientCommunicationRecord), id);
                }
                
                // Update follow-up settings
                client.UpdateCommunicationFollowUp(
                    id,
                    input.RequiresFollowUp,
                    input.FollowUpDate);
                
                // Save changes
                await _clientRepository.UpdateAsync(client);
                
                // Get updated record
                record = client.CommunicationRecords.First(r => r.Id == id);
                
                // Map to DTO
                var recordDto = ObjectMapper.Map<ClientCommunicationRecord, ClientCommunicationRecordDto>(record);
                
                // Set user name
                try
                {
                    var userName = _currentUser.UserName;
                    recordDto.UserName = userName;
                }
                catch
                {
                    recordDto.UserName = "Unknown User";
                }
                
                // Set project name if applicable
                if (record.ProjectId.HasValue)
                {
                    try
                    {
                        var project = await _projectRepository.GetAsync(record.ProjectId.Value);
                        recordDto.ProjectName = project.Name;
                    }
                    catch
                    {
                        recordDto.ProjectName = "Unknown Project";
                    }
                }
                
                return recordDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:CommunicationRecordNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in UpdateCommunicationFollowUpAsync for record {RecordId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Client.ManageCommunications)]
        public virtual async Task MarkCommunicationFollowUpCompletedAsync(Guid id)
        {
            try
            {
                // Find the client containing this communication record
                var clients = await _clientRepository.GetListAsync();
                FreelanceClient client = null;
                
                foreach (var c in clients)
                {
                    var r = c.CommunicationRecords.FirstOrDefault(r => r.Id == id);
                    if (r != null)
                    {
                        client = c;
                        break;
                    }
                }
                
                if (client == null)
                {
                    throw new EntityNotFoundException(typeof(ClientCommunicationRecord), id);
                }
                
                // Mark follow-up as completed
                client.MarkCommunicationFollowUpCompleted(id);
                
                // Save changes
                await _clientRepository.UpdateAsync(client);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:CommunicationRecordNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in MarkCommunicationFollowUpCompletedAsync for record {RecordId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Client.ManageCommunications)]
        public virtual async Task DeleteCommunicationRecordAsync(Guid id)
        {
            try
            {
                // Find the client containing this communication record
                var clients = await _clientRepository.GetListAsync();
                FreelanceClient client = null;
                
                foreach (var c in clients)
                {
                    if (c.CommunicationRecords.Any(r => r.Id == id))
                    {
                        client = c;
                        break;
                    }
                }
                
                if (client == null)
                {
                    return; // Record not found, nothing to delete
                }
                
                // Remove the record
                client.RemoveCommunicationRecord(id);
                
                // Save changes
                await _clientRepository.UpdateAsync(client);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in DeleteCommunicationRecordAsync for record {RecordId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }
        #endregion

        #region Follow-ups
        [Authorize(FreelanceOperationsPermissions.Client.View)]
        public virtual async Task<List<ClientCommunicationRecordDto>> GetPendingFollowUpsAsync(Guid clientId)
        {
            try
            {
                // Get client
                var client = await _clientRepository.GetAsync(clientId);
                
                // Get pending follow-ups
                var pendingFollowUps = client.GetPendingFollowUps();
                
                // Map to DTOs
                var followUpDtos = ObjectMapper.Map<List<ClientCommunicationRecord>, List<ClientCommunicationRecordDto>>(pendingFollowUps.ToList());
                
                // Get project names for records with project IDs
                var projectIds = followUpDtos
                    .Where(r => r.ProjectId.HasValue)
                    .Select(r => r.ProjectId.Value)
                    .Distinct()
                    .ToList();
                
                var projects = await _projectRepository.GetListAsync(p => projectIds.Contains(p.Id));
                
                foreach (var followUpDto in followUpDtos)
                {
                    // Set user name
                    try
                    {
                        var userName = _currentUser.UserName;
                        followUpDto.UserName = userName;
                    }
                    catch
                    {
                        followUpDto.UserName = "Unknown User";
                    }
                    
                    // Set project name if applicable
                    if (followUpDto.ProjectId.HasValue)
                    {
                        var project = projects.FirstOrDefault(p => p.Id == followUpDto.ProjectId.Value);
                        followUpDto.ProjectName = project?.Name ?? "Unknown Project";
                    }
                    
                    // Set overdue flag
                    followUpDto.IsFollowUpOverdue = followUpDto.RequiresFollowUp && 
                                                    followUpDto.FollowUpDate.HasValue && 
                                                    followUpDto.FollowUpDate.Value < DateTime.UtcNow;
                }
                
                return followUpDtos;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ClientNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetPendingFollowUpsAsync for client {ClientId}", clientId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Client.View)]
        public virtual async Task<List<FreelanceClientDto>> GetClientsWithPendingFollowUpsAsync()
        {
            try
            {
                // Get clients with pending follow-ups using domain service
                var clients = await _clientManager.GetClientsWithPendingFollowUpsAsync();
                
                // Map to DTOs
                var clientDtos = ObjectMapper.Map<List<FreelanceClient>, List<FreelanceClientDto>>(clients);
                
                // Get project counts
                foreach (var clientDto in clientDtos)
                {
                    var projectCount = await _projectRepository.CountAsync(p => p.ClientId == clientDto.Id);
                    clientDto.ProjectCount = projectCount;
                }
                
                return clientDtos;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetClientsWithPendingFollowUpsAsync");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }
        #endregion
    }
}
