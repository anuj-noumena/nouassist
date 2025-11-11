using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Data;
using NouAssist.WorkManagement.ProjectManagement.Permissions;
using NouAssist.WorkManagement.ProjectManagement.Sprints.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.ProjectManagement.Sprints
{
    [Authorize(ProjectsPermissions.Sprints.Default)]
    public class ProjectSprintAppService : ApplicationService, IProjectSprintAppService
    {
        private readonly IProjectSprintRepository _sprintRepository;

        public ProjectSprintAppService(IProjectSprintRepository sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }

        public async Task<ProjectSprintDto> GetAsync(Guid id)
        {
            var sprint = await _sprintRepository.GetAsync(id);
            return ObjectMapper.Map<ProjectSprint, ProjectSprintDto>(sprint);
        }

        public async Task<List<ProjectSprintDto>> GetByProjectAsync(Guid projectId)
        {
            var sprints = await _sprintRepository.GetByProjectIdAsync(projectId);
            return ObjectMapper.Map<List<ProjectSprint>, List<ProjectSprintDto>>(sprints);
        }

        public async Task<ProjectSprintDto> GetActiveSprintAsync(Guid projectId)
        {
            var sprint = await _sprintRepository.GetActiveSprintAsync(projectId);
            
            if (sprint == null)
            {
                throw new UserFriendlyException("No active sprint found for this project.");
            }

            return ObjectMapper.Map<ProjectSprint, ProjectSprintDto>(sprint);
        }

        [Authorize(ProjectsPermissions.Sprints.Create)]
        public async Task<ProjectSprintDto> CreateAsync(CreateSprintDto input)
        {
            var sprint = new ProjectSprint(
                GuidGenerator.Create(),
                input.ProjectId,
                input.Name,
                input.Goal,
                input.StartDate,
                input.EndDate,
                CurrentTenant.Id,
                input.VelocityEstimate);

            if (!string.IsNullOrWhiteSpace(input.ReleaseTheme))
            {
                sprint.ReleaseTheme = input.ReleaseTheme;
            }

            await _sprintRepository.InsertAsync(sprint);

            return ObjectMapper.Map<ProjectSprint, ProjectSprintDto>(sprint);
        }

        [Authorize(ProjectsPermissions.Sprints.Update)]
        public async Task<ProjectSprintDto> UpdateAsync(Guid id, UpdateSprintDto input)
        {
            var sprint = await _sprintRepository.GetAsync(id);
            
            sprint.Name = input.Name;
            sprint.Goal = input.Goal;
            sprint.ReleaseTheme = input.ReleaseTheme;
            sprint.SetDates(input.StartDate, input.EndDate);
            
            await _sprintRepository.UpdateAsync(sprint);

            return ObjectMapper.Map<ProjectSprint, ProjectSprintDto>(sprint);
        }

        [Authorize(ProjectsPermissions.Sprints.Start)]
        public async Task<ProjectSprintDto> StartAsync(Guid id)
        {
            var sprint = await _sprintRepository.GetAsync(id);
            
            sprint.Start();
            
            await _sprintRepository.UpdateAsync(sprint);

            return ObjectMapper.Map<ProjectSprint, ProjectSprintDto>(sprint);
        }

        [Authorize(ProjectsPermissions.Sprints.Complete)]
        public async Task<ProjectSprintDto> CompleteAsync(Guid id, CompleteSprintDto input)
        {
            var sprint = await _sprintRepository.GetAsync(id);
            
            var completedBy = CurrentUser.UserName ?? CurrentUser.Id?.ToString() ?? "System";
            sprint.Complete(completedBy, input.RetrospectiveNotes);
            
            await _sprintRepository.UpdateAsync(sprint);

            return ObjectMapper.Map<ProjectSprint, ProjectSprintDto>(sprint);
        }

        [Authorize(ProjectsPermissions.Sprints.Update)]
        public async Task<ProjectSprintDto> UpdateStoryPointsAsync(Guid id, UpdateSprintStoryPointsDto input)
        {
            var sprint = await _sprintRepository.GetAsync(id);
            
            sprint.UpdateStoryPoints(input.PlannedStoryPoints, input.CompletedStoryPoints);
            
            await _sprintRepository.UpdateAsync(sprint);

            return ObjectMapper.Map<ProjectSprint, ProjectSprintDto>(sprint);
        }

        [Authorize(ProjectsPermissions.Sprints.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _sprintRepository.DeleteAsync(id);
        }
    }
}
