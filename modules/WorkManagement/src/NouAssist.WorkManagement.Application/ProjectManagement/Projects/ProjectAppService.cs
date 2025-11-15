using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Data;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using NouAssist.WorkManagement.ProjectManagement.Projects.Dtos;
using NouAssist.WorkManagement.ProjectManagement.SuccessCriteria.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.ProjectManagement.Projects
{
    [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Default)]
    public class ProjectAppService : ApplicationService, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectAppService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectDto> GetAsync(Guid id)
        {
            var project = await _projectRepository.GetWithDetailsAsync(id);
            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        public async Task<PagedResultDto<ProjectDto>> GetListAsync(GetProjectListDto input)
        {
            var queryable = await _projectRepository.GetQueryableAsync();

            // Apply filters
            if (input.OwnerId.HasValue)
            {
                queryable = queryable.Where(p => p.OwnerId == input.OwnerId.Value);
            }

            if (input.AreaId.HasValue)
            {
                queryable = queryable.Where(p => p.AreaId == input.AreaId.Value);
            }

            if (input.GoalId.HasValue)
            {
                queryable = queryable.Where(p => p.GoalId == input.GoalId.Value);
            }

            if (input.Status.HasValue)
            {
                queryable = queryable.Where(p => p.Status == input.Status.Value);
            }

            if (input.IsArchived.HasValue)
            {
                queryable = queryable.Where(p => p.IsArchived == input.IsArchived.Value);
            }
            else
            {
                // Default: exclude archived
                queryable = queryable.Where(p => !p.IsArchived);
            }

            if (input.IsOverdue == true)
            {
                var now = DateTime.UtcNow;
                queryable = queryable.Where(p =>
                    p.Timeline.PlannedDueDate != null &&
                    p.Timeline.PlannedDueDate.Value < now &&
                    p.Status != ProjectStatus.Completed &&
                    p.Status != ProjectStatus.Cancelled);
            }

            if (input.PlannedDueAfter.HasValue)
            {
                queryable = queryable.Where(p =>
                    p.Timeline.PlannedDueDate != null &&
                    p.Timeline.PlannedDueDate.Value >= input.PlannedDueAfter.Value);
            }

            if (input.PlannedDueBefore.HasValue)
            {
                queryable = queryable.Where(p =>
                    p.Timeline.PlannedDueDate != null &&
                    p.Timeline.PlannedDueDate.Value <= input.PlannedDueBefore.Value);
            }

            if (input.PlannedStartAfter.HasValue)
            {
                queryable = queryable.Where(p =>
                    p.Timeline.PlannedStartDate != null &&
                    p.Timeline.PlannedStartDate.Value >= input.PlannedStartAfter.Value);
            }

            if (input.PlannedStartBefore.HasValue)
            {
                queryable = queryable.Where(p =>
                    p.Timeline.PlannedStartDate != null &&
                    p.Timeline.PlannedStartDate.Value <= input.PlannedStartBefore.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(p =>
                    p.Title.Contains(input.Filter) ||
                    (p.Description != null && p.Description.Contains(input.Filter)) ||
                    (p.Vision != null && p.Vision.Contains(input.Filter)));
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                // Note: Sorting by nested properties needs special handling
                // You may need to map "PlannedDueDate" to "Timeline.PlannedDueDate"
                queryable = queryable.OrderBy(input.Sorting.Replace("PlannedDueDate", "Timeline.PlannedDueDate")
                    .Replace("PlannedStartDate", "Timeline.PlannedStartDate"));
            }
            else
            {
                queryable = queryable.OrderByDescending(p => p.CreationTime);
            }

            // Apply paging
            var projects = await AsyncExecuter.ToListAsync(
                queryable.PageBy(input.SkipCount, input.MaxResultCount)
            );

            return new PagedResultDto<ProjectDto>(
                totalCount,
                ObjectMapper.Map<List<Project>, List<ProjectDto>>(projects)
            );
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Create)]
        public async Task<ProjectDto> CreateAsync(CreateProjectDto input)
        {
            var project = new Project(
                GuidGenerator.Create(),
                input.OwnerId,
                input.Title,
                CurrentTenant.Id,
                input.AreaId,
                input.Description,
                input.ExpectedOutcome,
                input.StartDate,
                input.DueDate,
                input.Importance,
                input.Urgency);

            project.Vision = input.Vision;
            project.Purpose = input.Purpose;
            project.GoalId = input.GoalId;
            project.PrimaryTeamId = input.PrimaryTeamId;

            await _projectRepository.InsertAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Update)]
        public async Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input)
        {
            var project = await _projectRepository.GetAsync(id);

            // Update basic properties - note these are now private set, 
            // but you might need to add public setters or methods
            // For now, assuming properties have internal/public setters
            project.Description = input.Description;
            project.Vision = input.Vision;
            project.Purpose = input.Purpose;
            project.ExpectedOutcome = input.ExpectedOutcome;
            project.WorkflowSchemeId = input.WorkflowSchemeId;
            project.AreaId = input.AreaId;
            project.PrimaryTeamId = input.PrimaryTeamId;

            project.UpdateTimeline(input.StartDate, input.DueDate);
            project.UpdatePriority(input.Importance, input.Urgency);
            project.UpdateProgress(input.Progress);

            await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _projectRepository.DeleteAsync(id);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Update)]
        public async Task<ProjectDto> StartAsync(Guid id)
        {
            var project = await _projectRepository.GetAsync(id);

            project.Start();

            await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Complete)]
        public async Task<ProjectDto> CompleteAsync(Guid id)
        {
            var project = await _projectRepository.GetAsync(id);

            var completedBy = CurrentUser.UserName ?? CurrentUser.Id?.ToString() ?? "System";
            project.Complete(completedBy);

            await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Cancel)]
        public async Task<ProjectDto> CancelAsync(Guid id)
        {
            var project = await _projectRepository.GetAsync(id);

            project.Cancel();

            await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Update)]
        public async Task<ProjectDto> PauseAsync(Guid id)
        {
            var project = await _projectRepository.GetAsync(id);

            project.Pause();

            await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Archive)]
        public async Task<ProjectDto> ArchiveAsync(Guid id)
        {
            var project = await _projectRepository.GetAsync(id);

            project.Archive();

            await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Update)]
        public async Task<ProjectDto> UpdateProgressAsync(Guid id, UpdateProjectProgressDto input)
        {
            var project = await _projectRepository.GetAsync(id);

            project.UpdateProgress(input.Progress);

            await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Update)]
        public async Task<ProjectDto> UpdatePriorityAsync(Guid id, UpdateProjectPriorityDto input)
        {
            var project = await _projectRepository.GetAsync(id);

            project.UpdatePriority(input.Importance, input.Urgency);

            await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }
        
        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Update)]
        public async Task<ProjectDto> AddSuccessCriterionAsync(Guid id, AddSuccessCriterionToProjectDto input)
        {
            var project = await _projectRepository.GetAsync(id);
    
            project.AddSuccessCriterion(
                GuidGenerator.Create(),
                input.Title,
                input.CriterionType,
                input.Description,
                input.MeasurementMethod,
                input.TargetValue,
                input.Weight);
    
            await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Update)]
        public async Task<ProjectDto> RemoveSuccessCriterionAsync(Guid id, Guid criterionId)
        {
            var project = await _projectRepository.GetAsync(id);
    
            project.RemoveSuccessCriterion(criterionId);
    
            await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Update)]
        public async Task<ProjectDto> MarkCriterionAsMetAsync(Guid id, Guid criterionId, MarkCriterionAsMetDto input)
        {
            var project = await _projectRepository.GetAsync(id);
    
            var metBy = CurrentUser.UserName ?? CurrentUser.Id?.ToString() ?? "System";
            project.MarkCriterionAsMet(criterionId, metBy, input.Notes);
    
            await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }
    }
}