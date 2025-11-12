using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.ProjectManagement.Data;
using NouAssist.WorkManagement.ProjectManagement.Dependencies.Dtos;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using NouAssist.WorkManagement.ProjectManagement.Services;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.ProjectManagement.Dependencies
{
    [Authorize(WorkManagementPermissions.ProjectManagement.Dependencies.Default)]
    public class ProjectDependencyAppService : ApplicationService, IProjectDependencyAppService
    {
        private readonly IProjectDependencyRepository _dependencyRepository;
        private readonly ProjectDependencyManagementService _dependencyService;

        public ProjectDependencyAppService(
            IProjectDependencyRepository dependencyRepository,
            ProjectDependencyManagementService dependencyService)
        {
            _dependencyRepository = dependencyRepository;
            _dependencyService = dependencyService;
        }

        public async Task<ProjectDependencyDto> GetAsync(Guid id)
        {
            var dependency = await _dependencyRepository.GetAsync(id);
            return ObjectMapper.Map<ProjectDependency, ProjectDependencyDto>(dependency);
        }

        public async Task<List<ProjectDependencyDto>> GetByProjectAsync(Guid projectId)
        {
            var dependencies = await _dependencyRepository.GetByProjectIdAsync(projectId);
            return ObjectMapper.Map<List<ProjectDependency>, List<ProjectDependencyDto>>(dependencies);
        }

        public async Task<List<ProjectDependencyDto>> GetBlockingDependenciesAsync(Guid projectId)
        {
            var dependencies = await _dependencyRepository.GetBlockingDependenciesAsync(projectId);
            return ObjectMapper.Map<List<ProjectDependency>, List<ProjectDependencyDto>>(dependencies);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Dependencies.Create)]
        public async Task<ProjectDependencyDto> CreateAsync(CreateDependencyDto input)
        {
            // Validate against circular dependencies
            var allDependencies = await _dependencyRepository.GetListAsync();
            var validationResult = _dependencyService.ValidateDependencyChain(
                input.ProjectId,
                input.DependsOnProjectId,
                allDependencies);

            if (!validationResult.IsValid)
            {
                throw new UserFriendlyException(
                    "Invalid dependency",
                    string.Join("; ", validationResult.Errors));
            }

            var dependency = new ProjectDependency(
                GuidGenerator.Create(),
                input.ProjectId,
                input.DependsOnProjectId,
                input.DependencyType,
                CurrentTenant.Id,
                input.LagDays,
                input.IsBlocking,
                input.Description);

            if (input.ExpectedResolutionDate.HasValue)
            {
                dependency.ExpectedResolutionDate = input.ExpectedResolutionDate;
            }

            await _dependencyRepository.InsertAsync(dependency);

            return ObjectMapper.Map<ProjectDependency, ProjectDependencyDto>(dependency);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Dependencies.Update)]
        public async Task<ProjectDependencyDto> UpdateAsync(Guid id, UpdateDependencyDto input)
        {
            var dependency = await _dependencyRepository.GetAsync(id);
            
            dependency.Description = input.Description;
            dependency.Notes = input.Notes;
            dependency.LagDays = input.LagDays;
            dependency.IsBlocking = input.IsBlocking;
            dependency.ExpectedResolutionDate = input.ExpectedResolutionDate;
            
            await _dependencyRepository.UpdateAsync(dependency);

            return ObjectMapper.Map<ProjectDependency, ProjectDependencyDto>(dependency);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Dependencies.Resolve)]
        public async Task<ProjectDependencyDto> ResolveAsync(Guid id)
        {
            var dependency = await _dependencyRepository.GetAsync(id);
            
            var resolvedBy = CurrentUser.UserName ?? CurrentUser.Id?.ToString() ?? "System";
            dependency.Resolve(resolvedBy);
            
            await _dependencyRepository.UpdateAsync(dependency);

            return ObjectMapper.Map<ProjectDependency, ProjectDependencyDto>(dependency);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Dependencies.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _dependencyRepository.DeleteAsync(id);
        }
    }
}
