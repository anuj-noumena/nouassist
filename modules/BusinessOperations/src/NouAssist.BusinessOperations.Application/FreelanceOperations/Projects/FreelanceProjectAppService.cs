using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
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
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.BusinessOperations.FreelanceOperations.Projects
{
    [Authorize(FreelanceOperationsPermissions.Project.Default)]
    public class FreelanceProjectAppService : ApplicationService, IFreelanceProjectAppService
    {
        private readonly IFreelanceProjectRepository _projectRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IFreelanceProjectManager _projectManager;
        private readonly ICurrentUser _currentUser;
        private readonly IClock _clock;
        
        public FreelanceProjectAppService(
            IFreelanceProjectRepository projectRepository,
            IClientRepository clientRepository,
            IFreelanceProjectManager projectManager,
            ICurrentUser currentUser,
            IClock clock)
        {
            _projectRepository = projectRepository;
            _clientRepository = clientRepository;
            _projectManager = projectManager;
            _currentUser = currentUser;
            _clock = clock;
            
            LocalizationResource = typeof(FreelanceOperationsResource);
        }

        [Authorize(FreelanceOperationsPermissions.Project.View)]
        public virtual async Task<FreelanceProjectDto> GetAsync(Guid id)
        {
            try
            {
                // Get project with details
                var project = await _projectRepository.GetAsync(id);
                
                // Map to DTO
                var projectDto = ObjectMapper.Map<FreelanceProject, FreelanceProjectDto>(project);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(project.ClientId);
                    projectDto.ClientName = client.Name;
                }
                catch
                {
                    // If client not found, just leave client name empty
                    projectDto.ClientName = "Unknown Client";
                }
                
                return projectDto;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ProjectNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetAsync for project {ProjectId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.View)]
        public virtual async Task<PagedResultDto<FreelanceProjectDto>> GetListAsync(GetFreelanceProjectListDto input)
        {
            try
            {
                // Prepare query
                var query = await _projectRepository.GetQueryableAsync();
                
                // Apply filters
                if (!string.IsNullOrEmpty(input.Filter))
                {
                    query = query.Where(p => p.Name.Contains(input.Filter));
                }
                
                if (input.ClientId.HasValue)
                {
                    query = query.Where(p => p.ClientId == input.ClientId.Value);
                }
                
                if (input.Status.HasValue)
                {
                    query = query.Where(p => p.Status == input.Status.Value);
                }
                
                if (input.BillingType.HasValue)
                {
                    query = query.Where(p => p.BillingType == input.BillingType.Value);
                }
                
                if (input.StartDateMin.HasValue)
                {
                    query = query.Where(p => p.StartDate >= input.StartDateMin.Value);
                }
                
                if (input.StartDateMax.HasValue)
                {
                    query = query.Where(p => p.StartDate <= input.StartDateMax.Value);
                }
                
                if (input.EndDateMin.HasValue)
                {
                    query = query.Where(p => p.EndDate >= input.EndDateMin.Value);
                }
                
                if (input.EndDateMax.HasValue)
                {
                    query = query.Where(p => p.EndDate <= input.EndDateMax.Value);
                }
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting
                query = query.OrderBy(input.Sorting ?? nameof(FreelanceProject.Name));
                
                // Apply paging
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
                
                // Execute query
                var projects = await AsyncExecuter.ToListAsync(query);
                
                // Map to DTOs
                var projectDtos = ObjectMapper.Map<List<FreelanceProject>, List<FreelanceProjectDto>>(projects);
                
                // Get client names
                var clientIds = projectDtos.Select(p => p.ClientId).Distinct().ToList();
                var clients = await _clientRepository.GetListAsync(c => clientIds.Contains(c.Id));
                
                foreach (var projectDto in projectDtos)
                {
                    var client = clients.FirstOrDefault(c => c.Id == projectDto.ClientId);
                    projectDto.ClientName = client?.Name ?? "Unknown Client";
                }
                
                return new PagedResultDto<FreelanceProjectDto>(totalCount, projectDtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetListAsync for projects");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.Create)]
        public virtual async Task<FreelanceProjectDto> CreateAsync(CreateFreelanceProjectDto input)
        {
            try
            {
                // Create project using domain service
                FreelanceProject project;
                
                // Create scope description value object
                var scope = ObjectMapper.Map<CreateFreelanceProjectDto, ProjectScopeDescription>(input);
                
                // Create project based on billing type
                switch (input.BillingType)
                {
                    case Enums.BillingType.Hourly:
                        if (!input.HourlyRate.HasValue || string.IsNullOrEmpty(input.HourlyRateCurrency))
                        {
                            throw new BusinessException(L["FreelanceOperations:Validation:HourlyRateRequired"]);
                        }
                        
                        var hourlyRate = new HourlyRate(input.HourlyRate.Value, input.HourlyRateCurrency);
                        project = await _projectManager.CreateHourlyProjectAsync(
                            input.Name,
                            input.ClientId,
                            hourlyRate,
                            scope,
                            input.StartDate,
                            input.EndDate);
                        break;
                        
                    case Enums.BillingType.FixedPrice:
                        if (!input.FixedPrice.HasValue || string.IsNullOrEmpty(input.FixedPriceCurrency))
                        {
                            throw new BusinessException(L["FreelanceOperations:Validation:FixedPriceRequired"]);
                        }
                        
                        project = await _projectManager.CreateFixedPriceProjectAsync(
                            input.Name,
                            input.ClientId,
                            input.FixedPrice.Value,
                            input.FixedPriceCurrency,
                            scope,
                            input.StartDate,
                            input.EndDate);
                        break;
                        
                    case Enums.BillingType.Milestone:
                        project = await _projectManager.CreateMilestoneProjectAsync(
                            input.Name,
                            input.ClientId,
                            scope,
                            input.StartDate,
                            input.EndDate);
                        break;
                        
                    default:
                        throw new BusinessException(L["FreelanceOperations:Validation:InvalidBillingType"]);
                }
                
                // Map to DTO
                var projectDto = ObjectMapper.Map<FreelanceProject, FreelanceProjectDto>(project);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(project.ClientId);
                    projectDto.ClientName = client.Name;
                }
                catch
                {
                    // If client not found, just leave client name empty
                    projectDto.ClientName = "Unknown Client";
                }
                
                return projectDto;
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in CreateAsync for project");
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.Update)]
        public virtual async Task<FreelanceProjectDto> UpdateAsync(Guid id, UpdateFreelanceProjectDto input)
        {
            try
            {
                // Get project
                var project = await _projectRepository.GetAsync(id);
                
                // Create scope description value object
                var scope = new ProjectScopeDescription(
                    input.ScopeDescription,
                    input.ScopeDeliverables,
                    input.ScopeIsFlexible);
                
                // Update basic details
                project.UpdateBasicDetails(
                    input.Name,
                    scope,
                    input.StartDate,
                    input.EndDate);
                
                // Update billing specific details if provided
                if (project.BillingType == Enums.BillingType.Hourly && 
                    input.HourlyRate.HasValue && 
                    !string.IsNullOrEmpty(input.HourlyRateCurrency))
                {
                    project.UpdateHourlyRate(new HourlyRate(input.HourlyRate.Value, input.HourlyRateCurrency));
                }
                else if (project.BillingType == Enums.BillingType.FixedPrice && 
                         input.FixedPrice.HasValue && 
                         !string.IsNullOrEmpty(input.FixedPriceCurrency))
                {
                    project.UpdateFixedPrice(input.FixedPrice.Value, input.FixedPriceCurrency);
                }
                
                // Save changes
                await _projectRepository.UpdateAsync(project);
                
                // Map to DTO
                var projectDto = ObjectMapper.Map<FreelanceProject, FreelanceProjectDto>(project);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(project.ClientId);
                    projectDto.ClientName = client.Name;
                }
                catch
                {
                    // If client not found, just leave client name empty
                    projectDto.ClientName = "Unknown Client";
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
                Logger.LogError(ex, "Error in UpdateAsync for project {ProjectId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _projectRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException)
            {
                // Do nothing if entity not found
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in DeleteAsync for project {ProjectId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.Update)]
        public virtual async Task<FreelanceProjectDto> ChangeStatusAsync(Guid id, ChangeProjectStatusDto input)
        {
            try
            {
                // Change status using domain service
                await _projectManager.ChangeProjectStatusAsync(id, input.NewStatus);
                
                // Get updated project
                var project = await _projectRepository.GetAsync(id);
                
                // Map to DTO
                var projectDto = ObjectMapper.Map<FreelanceProject, FreelanceProjectDto>(project);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(project.ClientId);
                    projectDto.ClientName = client.Name;
                }
                catch
                {
                    // If client not found, just leave client name empty
                    projectDto.ClientName = "Unknown Client";
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
                Logger.LogError(ex, "Error in ChangeStatusAsync for project {ProjectId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.Update)]
        public virtual async Task<FreelanceProjectDto> LinkToProposalAsync(Guid id, Guid proposalId)
        {
            try
            {
                // Link to proposal using domain service
                await _projectManager.LinkProjectToProposalAsync(id, proposalId);
                
                // Get updated project
                var project = await _projectRepository.GetAsync(id);
                
                // Map to DTO
                var projectDto = ObjectMapper.Map<FreelanceProject, FreelanceProjectDto>(project);
                
                // Get client name
                try
                {
                    var client = await _clientRepository.GetAsync(project.ClientId);
                    projectDto.ClientName = client.Name;
                }
                catch
                {
                    // If client not found, just leave client name empty
                    projectDto.ClientName = "Unknown Client";
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
                Logger.LogError(ex, "Error in LinkToProposalAsync for project {ProjectId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.View)]
        public virtual async Task<decimal> CalculateTotalAmountAsync(Guid id)
        {
            try
            {
                return await _projectManager.CalculateTotalAmountAsync(id);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ProjectNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in CalculateTotalAmountAsync for project {ProjectId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        #region Time Entry Operations

        [Authorize(FreelanceOperationsPermissions.Project.ManageTimeEntries)]
        public virtual async Task<TimeEntryDto> GetTimeEntryAsync(Guid projectId, DateTime date, TimeSpan startTime)
        {
            try
            {
                // Get project with time entries
                var project = await _projectRepository.GetAsync(projectId);
                
                // Find the time entry
                var timeEntry = project.TimeEntries
                    .FirstOrDefault(t => t.Date.Date == date.Date && t.StartTime == startTime);
                
                if (timeEntry == null)
                {
                    throw new EntityNotFoundException(typeof(TimeEntry));
                }
                
                // Map to DTO
                return ObjectMapper.Map<TimeEntry, TimeEntryDto>(timeEntry);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:TimeEntryNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetTimeEntryAsync for project {ProjectId}, date {Date}, startTime {StartTime}", 
                    projectId, date, startTime);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.View)]
        public virtual async Task<List<TimeEntryDto>> GetTimeEntriesAsync(Guid projectId)
        {
            try
            {
                // Get project with time entries
                var project = await _projectRepository.GetAsync(projectId);
                
                // Map to DTOs
                return ObjectMapper.Map<List<TimeEntry>, List<TimeEntryDto>>(project.TimeEntries.ToList());
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ProjectNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetTimeEntriesAsync for project {ProjectId}", projectId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.ManageTimeEntries)]
        public virtual async Task<TimeEntryDto> AddTimeEntryAsync(Guid projectId, CreateTimeEntryDto input)
        {
            try
            {
                // Add time entry using domain service
                var timeEntry = await _projectManager.AddTimeEntryAsync(
                    projectId,
                    input.Date,
                    input.StartTime,
                    input.DurationMinutes,
                    input.Description,
                    input.IsBillable);
                
                // Map to DTO
                return ObjectMapper.Map<TimeEntry, TimeEntryDto>(timeEntry);
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
                Logger.LogError(ex, "Error in AddTimeEntryAsync for project {ProjectId}", projectId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.ManageTimeEntries)]
        public virtual async Task<TimeEntryDto> UpdateTimeEntryAsync(Guid projectId, DateTime date, TimeSpan startTime, UpdateTimeEntryDto input)
        {
            try
            {
                // Get project
                var project = await _projectRepository.GetAsync(projectId);
                
                // Update time entry
                project.UpdateTimeEntry(
                    date,
                    startTime,
                    input.DurationMinutes,
                    input.Description);
                
                // Get the updated time entry
                var timeEntry = project.TimeEntries
                    .FirstOrDefault(t => t.Date.Date == date.Date && t.StartTime == startTime);
                
                if (timeEntry == null)
                {
                    throw new EntityNotFoundException(typeof(TimeEntry));
                }
                
                // Set billable flag if different
                if (timeEntry.IsBillable != input.IsBillable)
                {
                    timeEntry.SetBillable(input.IsBillable);
                }
                
                // Save changes
                await _projectRepository.UpdateAsync(project);
                
                // Map to DTO
                return ObjectMapper.Map<TimeEntry, TimeEntryDto>(timeEntry);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:TimeEntryNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in UpdateTimeEntryAsync for project {ProjectId}, date {Date}, startTime {StartTime}", 
                    projectId, date, startTime);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.ManageTimeEntries)]
        public virtual async Task DeleteTimeEntryAsync(Guid projectId, DateTime date, TimeSpan startTime)
        {
            try
            {
                // Get project
                var project = await _projectRepository.GetAsync(projectId);
                
                // Remove time entry
                project.RemoveTimeEntry(date, startTime);
                
                // Save changes
                await _projectRepository.UpdateAsync(project);
            }
            catch (EntityNotFoundException)
            {
                // Do nothing if project not found
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in DeleteTimeEntryAsync for project {ProjectId}, date {Date}, startTime {StartTime}", 
                    projectId, date, startTime);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }
        #endregion

        #region Milestone Operations
        [Authorize(FreelanceOperationsPermissions.Project.View)]
        public virtual async Task<MilestoneDto> GetMilestoneAsync(Guid id)
        {
            try
            {
                // Get project with milestones
                var projects = await _projectRepository.GetListAsync();
                
                // Find the project containing this milestone
                foreach (var project in projects)
                {
                    var milestone = project.Milestones.FirstOrDefault(m => m.Id == id);
                    if (milestone != null)
                    {
                        // Map to DTO
                        return ObjectMapper.Map<ProjectMilestone, MilestoneDto>(milestone);
                    }
                }
                
                throw new EntityNotFoundException(typeof(ProjectMilestone), id);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:MilestoneNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetMilestoneAsync for milestone {MilestoneId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.View)]
        public virtual async Task<List<MilestoneDto>> GetMilestonesAsync(Guid projectId)
        {
            try
            {
                // Get project with milestones
                var project = await _projectRepository.GetAsync(projectId);
                
                // Map to DTOs
                return ObjectMapper.Map<List<ProjectMilestone>, List<MilestoneDto>>(project.Milestones.ToList());
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:ProjectNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in GetMilestonesAsync for project {ProjectId}", projectId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.ManageMilestones)]
        public virtual async Task<MilestoneDto> AddMilestoneAsync(Guid projectId, CreateMilestoneDto input)
        {
            try
            {
                // Create milestone details value object
                var details = new MilestoneDetails(
                    input.Title,
                    input.Description,
                    input.Price,
                    input.Currency,
                    input.AcceptanceCriteria);
                
                // Add milestone using domain service
                var milestone = await _projectManager.AddMilestoneAsync(
                    projectId,
                    details,
                    input.Deadline);
                
                // Map to DTO
                return ObjectMapper.Map<ProjectMilestone, MilestoneDto>(milestone);
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
                Logger.LogError(ex, "Error in AddMilestoneAsync for project {ProjectId}", projectId);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.ManageMilestones)]
        public virtual async Task<MilestoneDto> UpdateMilestoneAsync(Guid id, UpdateMilestoneDto input)
        {
            try
            {
                // Find the project containing this milestone
                var projects = await _projectRepository.GetListAsync();
                ProjectMilestone milestone = null;
                FreelanceProject project = null;
                
                foreach (var p in projects)
                {
                    var m = p.Milestones.FirstOrDefault(m => m.Id == id);
                    if (m != null)
                    {
                        milestone = m;
                        project = p;
                        break;
                    }
                }
                
                if (milestone == null || project == null)
                {
                    throw new EntityNotFoundException(typeof(ProjectMilestone), id);
                }
                
                // Create milestone details value object
                var details = new MilestoneDetails(
                    input.Title,
                    input.Description,
                    input.Price,
                    input.Currency,
                    input.AcceptanceCriteria);
                
                // Update milestone
                project.UpdateMilestone(
                    id,
                    details,
                    input.Deadline);
                
                // Save changes
                await _projectRepository.UpdateAsync(project);
                
                // Get updated milestone
                milestone = project.Milestones.First(m => m.Id == id);
                
                // Map to DTO
                return ObjectMapper.Map<ProjectMilestone, MilestoneDto>(milestone);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:MilestoneNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in UpdateMilestoneAsync for milestone {MilestoneId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.ManageMilestones)]
        public virtual async Task<MilestoneDto> CompleteMilestoneAsync(Guid id)
        {
            try
            {
                // Find the project containing this milestone
                var projects = await _projectRepository.GetListAsync();
                ProjectMilestone milestone = null;
                FreelanceProject project = null;
                
                foreach (var p in projects)
                {
                    var m = p.Milestones.FirstOrDefault(m => m.Id == id);
                    if (m != null)
                    {
                        milestone = m;
                        project = p;
                        break;
                    }
                }
                
                if (milestone == null || project == null)
                {
                    throw new EntityNotFoundException(typeof(ProjectMilestone), id);
                }
                
                // Complete milestone
                project.CompleteMilestone(id, _clock.Now);
                
                // Save changes
                await _projectRepository.UpdateAsync(project);
                
                // Get updated milestone
                milestone = project.Milestones.First(m => m.Id == id);
                
                // Map to DTO
                return ObjectMapper.Map<ProjectMilestone, MilestoneDto>(milestone);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:MilestoneNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in CompleteMilestoneAsync for milestone {MilestoneId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.ManageMilestones)]
        public virtual async Task<MilestoneDto> ReopenMilestoneAsync(Guid id)
        {
            try
            {
                // Find the project containing this milestone
                var projects = await _projectRepository.GetListAsync();
                ProjectMilestone milestone = null;
                FreelanceProject project = null;
                
                foreach (var p in projects)
                {
                    var m = p.Milestones.FirstOrDefault(m => m.Id == id);
                    if (m != null)
                    {
                        milestone = m;
                        project = p;
                        break;
                    }
                }
                
                if (milestone == null || project == null)
                {
                    throw new EntityNotFoundException(typeof(ProjectMilestone), id);
                }
                
                // Reopen milestone
                project.ReopenMilestone(id);
                
                // Save changes
                await _projectRepository.UpdateAsync(project);
                
                // Get updated milestone
                milestone = project.Milestones.First(m => m.Id == id);
                
                // Map to DTO
                return ObjectMapper.Map<ProjectMilestone, MilestoneDto>(milestone);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:MilestoneNotFound"]);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in ReopenMilestoneAsync for milestone {MilestoneId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FreelanceOperationsPermissions.Project.ManageMilestones)]
        public virtual async Task DeleteMilestoneAsync(Guid id)
        {
            try
            {
                // Find the project containing this milestone
                var projects = await _projectRepository.GetListAsync();
                FreelanceProject project = null;
                
                foreach (var p in projects)
                {
                    if (p.Milestones.Any(m => m.Id == id))
                    {
                        project = p;
                        break;
                    }
                }
                
                if (project == null)
                {
                    return; // Milestone not found, nothing to delete
                }
                
                // Remove milestone
                project.RemoveMilestone(id);
                
                // Save changes
                await _projectRepository.UpdateAsync(project);
            }
            catch (BusinessException)
            {
                throw; // Re-throw business exceptions
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in DeleteMilestoneAsync for milestone {MilestoneId}", id);
                throw new BusinessException(L["FreelanceOperations:ErrorMessage:GenericError"]);
            }
        }
        #endregion
    }
}
