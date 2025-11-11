using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.CoreProductivity.ProjectManagement.Data;
using NouAssist.CoreProductivity.ProjectManagement.Entities;
using NouAssist.CoreProductivity.ProjectManagement.Permissions;
using NouAssist.CoreProductivity.ProjectManagement.Requirements.Dtos;
using NouAssist.Workflow.Entities;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.ProjectManagement.Requirements
{
    [Authorize(ProjectsPermissions.Requirements.Default)]
    public class ProjectRequirementAppService : ApplicationService, IProjectRequirementAppService
    {
        private readonly IProjectRequirementRepository _requirementRepository;

        public ProjectRequirementAppService(IProjectRequirementRepository requirementRepository)
        {
            _requirementRepository = requirementRepository;
        }

        public async Task<ProjectRequirementDto> GetAsync(Guid id)
        {
            var requirement = await _requirementRepository.GetAsync(id);
            return ObjectMapper.Map<ProjectRequirement, ProjectRequirementDto>(requirement);
        }

        public async Task<List<ProjectRequirementDto>> GetByProjectAsync(Guid projectId)
        {
            var requirements = await _requirementRepository.GetByProjectIdAsync(projectId);
            return ObjectMapper.Map<List<ProjectRequirement>, List<ProjectRequirementDto>>(requirements);
        }

        public async Task<List<ProjectRequirementDto>> GetPendingApprovalAsync()
        {
            var requirements = await _requirementRepository.GetPendingApprovalAsync();
            return ObjectMapper.Map<List<ProjectRequirement>, List<ProjectRequirementDto>>(requirements);
        }

        [Authorize(ProjectsPermissions.Requirements.Create)]
        public async Task<ProjectRequirementDto> CreateAsync(CreateRequirementDto input)
        {
            var requirement = new ProjectRequirement(
                GuidGenerator.Create(),
                input.ProjectId,
                input.Title,
                input.RequirementType,
                input.Description);

            await _requirementRepository.InsertAsync(requirement);

            return ObjectMapper.Map<ProjectRequirement, ProjectRequirementDto>(requirement);
        }

        [Authorize(ProjectsPermissions.Requirements.Update)]
        public async Task<ProjectRequirementDto> UpdateAsync(Guid id, UpdateRequirementDto input)
        {
            var requirement = await _requirementRepository.GetAsync(id);

            requirement.Title = input.Title;
            requirement.Description = input.Description;

            await _requirementRepository.UpdateAsync(requirement);

            return ObjectMapper.Map<ProjectRequirement, ProjectRequirementDto>(requirement);
        }

        [Authorize(ProjectsPermissions.Requirements.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _requirementRepository.DeleteAsync(id);
        }
    }
}