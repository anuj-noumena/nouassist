using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.ProjectManagement.Data;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using NouAssist.WorkManagement.ProjectManagement.Requirements.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.ProjectManagement.Requirements
{
    [Authorize(WorkManagementPermissions.ProjectManagement.Requirements.Default)]
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

        [Authorize(WorkManagementPermissions.ProjectManagement.Requirements.Create)]
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

        [Authorize(WorkManagementPermissions.ProjectManagement.Requirements.Update)]
        public async Task<ProjectRequirementDto> UpdateAsync(Guid id, UpdateRequirementDto input)
        {
            var requirement = await _requirementRepository.GetAsync(id);

            requirement.Title = input.Title;
            requirement.Description = input.Description;

            await _requirementRepository.UpdateAsync(requirement);

            return ObjectMapper.Map<ProjectRequirement, ProjectRequirementDto>(requirement);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Requirements.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _requirementRepository.DeleteAsync(id);
        }
    }
}