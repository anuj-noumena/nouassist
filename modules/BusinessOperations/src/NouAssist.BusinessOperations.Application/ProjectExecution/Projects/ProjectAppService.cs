using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using NouAssist.BusinessOperations.ProjectExecution.Permissions;
using NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects
{
    [Authorize(ProjectExecutionPermissions.Projects.Default)]
    public class ProjectAppService : 
        CrudAppService<
            Project,
            ProjectDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateProjectDto,
            UpdateProjectDto>,
        IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IProgressTrackingService _progressTrackingService;
        private readonly IProjectTemplateService _projectTemplateService;
        
        public ProjectAppService(
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            IProgressTrackingService progressTrackingService,
            IProjectTemplateService projectTemplateService)
            : base(projectRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _progressTrackingService = progressTrackingService;
            _projectTemplateService = projectTemplateService;
            
            GetPolicyName = ProjectExecutionPermissions.Projects.Default;
            GetListPolicyName = ProjectExecutionPermissions.Projects.Default;
            CreatePolicyName = ProjectExecutionPermissions.Projects.Create;
            UpdatePolicyName = ProjectExecutionPermissions.Projects.Edit;
            DeletePolicyName = ProjectExecutionPermissions.Projects.Delete;
        }
        
        public override async Task<ProjectDto> GetAsync(Guid id)
        {
            var project = await _projectRepository.GetWithDetailsAsync(id, includePhases: true, includeMilestones: true, includeMembers: true);
            
            var dto = ObjectMapper.Map<Project, ProjectDto>(project);
            
            // Get completion percentage
            dto.CompletionPercentage = await _progressTrackingService.GetProjectCompletionPercentageAsync(id);
            
            return dto;
        }
        
        public async Task<ProjectDto> GetWithDetailsAsync(Guid id)
        {
            return await GetAsync(id);
        }
        
        public async Task<List<ProjectListDto>> GetListByOrganizationIdAsync(Guid organizationId)
        {
            var projects = await _projectRepository.GetListByOrganizationIdAsync(organizationId);
            
            var dtos = new List<ProjectListDto>();
            
            foreach (var project in projects)
            {
                var dto = ObjectMapper.Map<Project, ProjectListDto>(project);
                
                // Get completion percentage and task counts
                dto.CompletionPercentage = await _progressTrackingService.GetProjectCompletionPercentageAsync(project.Id);
                
                var tasks = await _taskRepository.GetListByProjectIdAsync(project.Id);
                dto.ActiveTaskCount = tasks.Count(t => t.Status != TaskStatus.Done);
                dto.CompletedTaskCount = tasks.Count(t => t.Status == TaskStatus.Done);
                
                dtos.Add(dto);
            }
            
            return dtos;
        }
        
        public async Task<List<ProjectListDto>> GetListByContactIdAsync(Guid contactId)
        {
            var projects = await _projectRepository.GetListByContactIdAsync(contactId);
            
            var dtos = new List<ProjectListDto>();
            
            foreach (var project in projects)
            {
                var dto = ObjectMapper.Map<Project, ProjectListDto>(project);
                
                // Get completion percentage and task counts
                dto.CompletionPercentage = await _progressTrackingService.GetProjectCompletionPercentageAsync(project.Id);
                
                var tasks = await _taskRepository.GetListByProjectIdAsync(project.Id);
                dto.ActiveTaskCount = tasks.Count(t => t.Status != TaskStatus.Done);
                dto.CompletedTaskCount = tasks.Count(t => t.Status == TaskStatus.Done);
                
                dtos.Add(dto);
            }
            
            return dtos;
        }
        
        public async Task<List<ProjectListDto>> GetListByStatusAsync(ProjectStatus status)
        {
            var projects = await _projectRepository.GetListByStatusAsync(status);
            
            var dtos = new List<ProjectListDto>();
            
            foreach (var project in projects)
            {
                var dto = ObjectMapper.Map<Project, ProjectListDto>(project);
                
                // Get completion percentage and task counts
                dto.CompletionPercentage = await _progressTrackingService.GetProjectCompletionPercentageAsync(project.Id);
                
                var tasks = await _taskRepository.GetListByProjectIdAsync(project.Id);
                dto.ActiveTaskCount = tasks.Count(t => t.Status != TaskStatus.Done);
                dto.CompletedTaskCount = tasks.Count(t => t.Status == TaskStatus.Done);
                
                dtos.Add(dto);
            }
            
            return dtos;
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.ManageStatus)]
        public async Task<ProjectDto> UpdateStatusAsync(Guid id, UpdateProjectStatusDto input)
        {
            var project = await _projectRepository.GetAsync(id);
            
            project.UpdateStatus(input.Status, input.ActualEndDate);
            
            await _projectRepository.UpdateAsync(project);
            
            return await GetAsync(id);
        }
        
        public override async Task<ProjectDto> CreateAsync(CreateProjectDto input)
        {
            var project = Project.Create(
                GuidGenerator.Create(),
                input.Name,
                input.Description,
                input.StartDate,
                input.PlannedEndDate,
                CurrentTenant.Id,
                input.OrganizationId,
                input.ContactId);
                
            await _projectRepository.InsertAsync(project);
            
            return await GetAsync(project.Id);
        }
        
        public override async Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input)
        {
            var project = await _projectRepository.GetAsync(id);
            
            project.UpdateDetails(
                input.Name,
                input.Description,
                input.StartDate,
                input.PlannedEndDate);
                
            if (input.OrganizationId.HasValue)
            {
                project.SetOrganization(input.OrganizationId.Value);
            }
            
            if (input.ContactId.HasValue)
            {
                project.SetContact(input.ContactId.Value);
            }
            
            await _projectRepository.UpdateAsync(project);
            
            return await GetAsync(id);
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.ManagePhases)]
        public async Task<ProjectPhaseDto> AddPhaseAsync(Guid id, CreateProjectPhaseDto input)
        {
            var project = await _projectRepository.GetAsync(id);
            
            var phase = project.AddPhase(
                input.Name,
                input.StartDate,
                input.EndDate,
                input.Order);
                
            await _projectRepository.UpdateAsync(project);
            
            return ObjectMapper.Map<ProjectPhase, ProjectPhaseDto>(phase);
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.ManageMilestones)]
        public async Task<ProjectMilestoneDto> AddMilestoneAsync(Guid id, CreateProjectMilestoneDto input)
        {
            var project = await _projectRepository.GetAsync(id);
            
            var milestone = project.AddMilestone(
                input.Name,
                input.TargetDate,
                input.Description);
                
            await _projectRepository.UpdateAsync(project);
            
            return ObjectMapper.Map<ProjectMilestone, ProjectMilestoneDto>(milestone);
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.ManageMilestones)]
        public async Task UpdateMilestoneStatusAsync(Guid id, UpdateMilestoneStatusDto input)
        {
            await _progressTrackingService.UpdateMilestoneStatusAsync(id, input.MilestoneId, input.IsCompleted);
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.ManageMembers)]
        public async Task<ProjectMemberDto> AddMemberAsync(Guid id, CreateProjectMemberDto input)
        {
            var project = await _projectRepository.GetAsync(id);
            
            var member = project.AddMember(
                input.UserId,
                input.Role,
                input.JoinDate);
                
            await _projectRepository.UpdateAsync(project);
            
            return ObjectMapper.Map<ProjectMember, ProjectMemberDto>(member);
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.ManageMembers)]
        public async Task RemoveMemberAsync(Guid id, Guid userId)
        {
            var project = await _projectRepository.GetAsync(id);
            
            project.RemoveMember(userId);
            
            await _projectRepository.UpdateAsync(project);
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.Create)]
        public async Task<ProjectDto> CreateFromTemplateAsync(CreateProjectFromTemplateDto input)
        {
            var project = await _projectTemplateService.CreateProjectFromTemplateAsync(
                input.TemplateName,
                input.ProjectName,
                input.ProjectDescription,
                input.StartDate,
                input.EndDate);
                
            return await GetAsync(project.Id);
        }
        
        public async Task<List<string>> GetAvailableTemplatesAsync()
        {
            return await _projectTemplateService.GetAvailableTemplatesAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Exceptions;
using NouAssist.BusinessOperations.ProjectExecution.Permissions;
using NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects
{
    [Authorize(ProjectExecutionPermissions.Projects.Default)]
    public class ProjectAppService : ApplicationService, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IProgressTrackingService _progressTrackingService;
        private readonly IProjectTemplateService _projectTemplateService;
        private readonly IIdentityUserLookupService _userLookupService;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<ProjectAppService> _logger;
        
        public ProjectAppService(
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            ITimeLogRepository timeLogRepository,
            IProgressTrackingService progressTrackingService,
            IProjectTemplateService projectTemplateService,
            IIdentityUserLookupService userLookupService,
            ICurrentUser currentUser,
            ILogger<ProjectAppService> logger)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _timeLogRepository = timeLogRepository;
            _progressTrackingService = progressTrackingService;
            _projectTemplateService = projectTemplateService;
            _userLookupService = userLookupService;
            _currentUser = currentUser;
            _logger = logger;
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.View)]
        public virtual async Task<ProjectDto> GetAsync(Guid id)
        {
            try
            {
                var project = await _projectRepository.GetAsync(id);
                
                var projectDto = ObjectMapper.Map<Project, ProjectDto>(project);
                
                projectDto.CompletionPercentage = await _progressTrackingService.GetProjectCompletionPercentageAsync(id);
                projectDto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByProjectIdAsync(id);
                
                return projectDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.View)]
        public virtual async Task<ProjectWithDetailsDto> GetWithDetailsAsync(Guid id)
        {
            try
            {
                var project = await _projectRepository.GetWithDetailsAsync(
                    id, 
                    includePhases: true, 
                    includeMilestones: true, 
                    includeMembers: true);
                
                var projectDto = ObjectMapper.Map<Project, ProjectWithDetailsDto>(project);
                
                projectDto.CompletionPercentage = await _progressTrackingService.GetProjectCompletionPercentageAsync(id);
                projectDto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByProjectIdAsync(id);
                
                // Get phase completion percentages
                foreach (var phase in projectDto.Phases)
                {
                    phase.CompletionPercentage = await _progressTrackingService.GetPhaseCompletionPercentageAsync(id, phase.ProjectId);
                }
                
                // Load user names for project members
                foreach (var member in projectDto.Members)
                {
                    try
                    {
                        var user = await _userLookupService.FindByIdAsync(member.UserId);
                        if (user != null)
                        {
                            member.UserName = user.Name;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not find user {UserId} for project member", member.UserId);
                        member.UserName = "Unknown User";
                    }
                }
                
                return projectDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.View)]
        public virtual async Task<PagedResultDto<ProjectDto>> GetListAsync(GetProjectListDto input)
        {
            try
            {
                var query = await _projectRepository.GetQueryableAsync();
                
                // Apply filters
                query = query.WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                    p => p.Details.Name.Contains(input.Filter) || 
                         p.Details.Description.Contains(input.Filter)
                );
                
                query = query.WhereIf(
                    input.Status.HasValue,
                    p => p.Status == input.Status
                );
                
                query = query.WhereIf(
                    input.OrganizationId.HasValue,
                    p => p.OrganizationId == input.OrganizationId
                );
                
                query = query.WhereIf(
                    input.ContactId.HasValue,
                    p => p.ContactId == input.ContactId
                );
                
                query = query.WhereIf(
                    input.StartDateFrom.HasValue,
                    p => p.Details.StartDate >= input.StartDateFrom.Value
                );
                
                query = query.WhereIf(
                    input.StartDateTo.HasValue,
                    p => p.Details.StartDate <= input.StartDateTo.Value
                );
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting
                query = query.OrderBy(!string.IsNullOrWhiteSpace(input.Sorting) 
                    ? input.Sorting 
                    : nameof(Project.CreationTime) + " desc");
                
                // Apply paging
                query = query.PageBy(input);
                
                // Execute query
                var projects = await AsyncExecuter.ToListAsync(query);
                
                // Map to DTOs
                var projectDtos = ObjectMapper.Map<List<Project>, List<ProjectDto>>(projects);
                
                // Load additional data for each project
                foreach (var projectDto in projectDtos)
                {
                    projectDto.CompletionPercentage = await _progressTrackingService.GetProjectCompletionPercentageAsync(projectDto.Id);
                    projectDto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByProjectIdAsync(projectDto.Id);
                }
                
                return new PagedResultDto<ProjectDto>(totalCount, projectDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.Create)]
        public virtual async Task<ProjectDto> CreateAsync(CreateProjectDto input)
        {
            try
            {
                // Create the project
                var project = Project.Create(
                    GuidGenerator.Create(),
                    input.Name,
                    input.Description,
                    input.StartDate,
                    input.PlannedEndDate,
                    CurrentTenant.Id,
                    input.OrganizationId,
                    input.ContactId
                );
                
                await _projectRepository.InsertAsync(project);
                
                // Map to DTO
                var projectDto = ObjectMapper.Map<Project, ProjectDto>(project);
                projectDto.CompletionPercentage = 0;
                projectDto.TotalLoggedHours = 0;
                
                return projectDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.Update)]
        public virtual async Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input)
        {
            try
            {
                var project = await _projectRepository.GetAsync(id);
                
                project.UpdateDetails(
                    input.Name,
                    input.Description,
                    input.StartDate,
                    input.PlannedEndDate
                );
                
                if (input.OrganizationId.HasValue)
                {
                    project.SetOrganization(input.OrganizationId.Value);
                }
                
                if (input.ContactId.HasValue)
                {
                    project.SetContact(input.ContactId.Value);
                }
                
                await _projectRepository.UpdateAsync(project);
                
                // Map to DTO
                var projectDto = ObjectMapper.Map<Project, ProjectDto>(project);
                projectDto.CompletionPercentage = await _progressTrackingService.GetProjectCompletionPercentageAsync(id);
                projectDto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByProjectIdAsync(id);
                
                return projectDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.Update)]
        public virtual async Task<ProjectDto> UpdateStatusAsync(Guid id, UpdateProjectStatusDto input)
        {
            try
            {
                var project = await _projectRepository.GetAsync(id);
                
                project.UpdateStatus(input.Status, input.ActualEndDate);
                
                await _projectRepository.UpdateAsync(project);
                
                // Map to DTO
                var projectDto = ObjectMapper.Map<Project, ProjectDto>(project);
                projectDto.CompletionPercentage = await _progressTrackingService.GetProjectCompletionPercentageAsync(id);
                projectDto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByProjectIdAsync(id);
                
                return projectDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _projectRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.ManagePhases)]
        public virtual async Task<ProjectPhaseDto> AddPhaseAsync(Guid projectId, CreateProjectPhaseDto input)
        {
            try
            {
                var project = await _projectRepository.GetWithDetailsAsync(projectId, includePhases: true);
                
                var phase = project.AddPhase(
                    input.Name,
                    input.StartDate,
                    input.EndDate,
                    input.Order
                );
                
                await _projectRepository.UpdateAsync(project);
                
                // Map to DTO
                var phaseDto = ObjectMapper.Map<ProjectPhase, ProjectPhaseDto>(phase);
                phaseDto.CompletionPercentage = 0;
                
                return phaseDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.ManageMilestones)]
        public virtual async Task<ProjectMilestoneDto> AddMilestoneAsync(Guid projectId, CreateProjectMilestoneDto input)
        {
            try
            {
                var project = await _projectRepository.GetWithDetailsAsync(projectId, includeMilestones: true);
                
                var milestone = project.AddMilestone(
                    input.Name,
                    input.TargetDate,
                    input.Description
                );
                
                await _projectRepository.UpdateAsync(project);
                
                // Map to DTO
                var milestoneDto = ObjectMapper.Map<ProjectMilestone, ProjectMilestoneDto>(milestone);
                
                return milestoneDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.ManageMilestones)]
        public virtual async Task UpdateMilestoneStatusAsync(Guid projectId, string milestoneName, UpdateProjectMilestoneStatusDto input)
        {
            try
            {
                await _progressTrackingService.UpdateMilestoneStatusAsync(
                    projectId,
                    Guid.Empty, // Not used in our implementation
                    input.IsCompleted
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.ManageMembers)]
        public virtual async Task<ProjectMemberDto> AddMemberAsync(Guid projectId, AddProjectMemberDto input)
        {
            try
            {
                var project = await _projectRepository.GetWithDetailsAsync(projectId, includeMembers: true);
                
                var member = project.AddMember(
                    input.UserId,
                    input.Role,
                    Clock.Now
                );
                
                await _projectRepository.UpdateAsync(project);
                
                // Map to DTO
                var memberDto = ObjectMapper.Map<ProjectMember, ProjectMemberDto>(member);
                
                // Load user name
                try
                {
                    var user = await _userLookupService.FindByIdAsync(input.UserId);
                    if (user != null)
                    {
                        memberDto.UserName = user.Name;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not find user {UserId} for project member", input.UserId);
                    memberDto.UserName = "Unknown User";
                }
                
                return memberDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.ManageMembers)]
        public virtual async Task RemoveMemberAsync(Guid projectId, Guid userId)
        {
            try
            {
                var project = await _projectRepository.GetWithDetailsAsync(projectId, includeMembers: true);
                
                project.RemoveMember(userId);
                
                await _projectRepository.UpdateAsync(project);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.View)]
        public virtual async Task<double> GetCompletionPercentageAsync(Guid projectId)
        {
            try
            {
                return await _progressTrackingService.GetProjectCompletionPercentageAsync(projectId);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.View)]
        public virtual async Task<double> GetTotalLoggedHoursAsync(Guid projectId)
        {
            try
            {
                return await _timeLogRepository.GetTotalHoursByProjectIdAsync(projectId);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.Create)]
        public virtual async Task<ProjectDto> CreateFromTemplateAsync(string templateName, CreateProjectDto input)
        {
            try
            {
                var project = await _projectTemplateService.CreateProjectFromTemplateAsync(
                    templateName,
                    input.Name,
                    input.Description,
                    input.StartDate,
                    input.PlannedEndDate
                );
                
                if (input.OrganizationId.HasValue)
                {
                    project.SetOrganization(input.OrganizationId.Value);
                }
                
                if (input.ContactId.HasValue)
                {
                    project.SetContact(input.ContactId.Value);
                }
                
                await _projectRepository.UpdateAsync(project);
                
                // Map to DTO
                var projectDto = ObjectMapper.Map<Project, ProjectDto>(project);
                projectDto.CompletionPercentage = 0;
                projectDto.TotalLoggedHours = 0;
                
                return projectDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Projects.View)]
        public virtual async Task<List<string>> GetAvailableTemplatesAsync()
        {
            try
            {
                return await _projectTemplateService.GetAvailableTemplatesAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "An error occurred in ProjectAppService: {Message}", exception.Message);
            
            if (exception is EntityNotFoundException entityNotFoundException)
            {
                throw new UserFriendlyException($"Project with ID {entityNotFoundException.Id} not found");
            }
            
            if (exception is ProjectExecutionDomainException domainException)
            {
                throw new UserFriendlyException(domainException.Message, domainException.Code, domainException.Data);
            }
        }
    }
}
