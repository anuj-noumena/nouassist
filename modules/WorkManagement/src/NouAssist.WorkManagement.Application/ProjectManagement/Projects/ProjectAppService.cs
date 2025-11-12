using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Data;
using NouAssist.WorkManagement.ProjectManagement.Projects.Dtos;
using NouAssist.WorkManagement.ProjectManagement.Specifications;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.ProjectManagement.Projects
{
    [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Default)]
    public class ProjectAppService : ApplicationService, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IRepository<Project, Guid> _genericProjectRepository;

        public ProjectAppService(
            IProjectRepository projectRepository,
            IRepository<Project, Guid> genericProjectRepository)
        {
            _projectRepository = projectRepository;
            _genericProjectRepository = genericProjectRepository;
        }

        public async Task<ProjectDto> GetAsync(Guid id)
        {
            var project = await _projectRepository.GetWithDetailsAsync(id);
            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        public async Task<PagedResultDto<ProjectDto>> GetListAsync(GetProjectListDto input)
        {
            var specification = CreateListSpecification(input);

            var projects = await _projectRepository.GetListAsync(
                specification,
                includeDetails: false);

            // Apply filtering, sorting, and paging
            var query = projects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(p => 
                    p.Title.Contains(input.Filter, StringComparison.OrdinalIgnoreCase) ||
                    (p.Description != null && p.Description.Contains(input.Filter, StringComparison.OrdinalIgnoreCase)));
            }

            var totalCount = query.Count();

            // Apply sorting using Dynamic LINQ
            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                query = query.OrderBy(input.Sorting);
            }
            else
            {
                query = query.OrderByDescending(p => p.CreationTime);
            }

            // Apply paging
            var pagedProjects = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<ProjectDto>(
                totalCount,
                ObjectMapper.Map<List<Project>, List<ProjectDto>>(pagedProjects)
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
                input.Description,
                input.ExpectedOutcome,
                input.StartDate,
                input.DueDate,
                input.Importance,
                input.Urgency);

            project.GoalId = input.GoalId;
            project.ContextId = input.ContextId;
            project.AreaId = input.AreaId;
            project.PrimaryTeamId = input.PrimaryTeamId;
            project.RequiresApproval = input.RequiresApproval;

            await _projectRepository.InsertAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Projects.Update)]
        public async Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input)
        {
            var project = await _projectRepository.GetAsync(id);

            project.Title = input.Title;
            project.Description = input.Description;
            project.Summary = input.Summary;
            project.ExpectedOutcome = input.ExpectedOutcome;
            project.WorkflowSchemeId = input.WorkflowSchemeId;
            project.ContextId = input.ContextId;
            project.AreaId = input.AreaId;
            project.PrimaryTeamId = input.PrimaryTeamId;

            project.UpdateDates(input.StartDate, input.DueDate);
            project.SetPriority(input.Importance, input.Urgency);
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
            
            project.SetPriority(input.Importance, input.Urgency);
            
            await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        private ISpecification<Project> CreateListSpecification(GetProjectListDto input)
        {
            ISpecification<Project> spec = new BaseSpecification<Project>();

            if (input.OwnerId.HasValue)
            {
                spec = spec.And(new ProjectsByOwnerSpecification(input.OwnerId.Value));
            }

            if (input.Status.HasValue)
            {
                spec = spec.And(new BaseSpecification<Project>(p => p.Status == input.Status.Value));
            }

            if (input.Quadrant.HasValue)
            {
                spec = spec.And(new ProjectsInQuadrantSpecification(input.Quadrant.Value));
            }

            if (input.IsArchived.HasValue)
            {
                spec = spec.And(new BaseSpecification<Project>(p => p.IsArchived == input.IsArchived.Value));
            }

            if (input.IsOverdue == true)
            {
                spec = spec.And(new OverdueProjectsSpecification());
            }

            if (input.DueAfter.HasValue)
            {
                spec = spec.And(new BaseSpecification<Project>(p => 
                    p.DueDate.HasValue && p.DueDate.Value >= input.DueAfter.Value));
            }

            if (input.DueBefore.HasValue)
            {
                spec = spec.And(new BaseSpecification<Project>(p => 
                    p.DueDate.HasValue && p.DueDate.Value <= input.DueBefore.Value));
            }

            return spec;
        }
        
    }
}
