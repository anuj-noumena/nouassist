using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Projects.Data;
using NouAssist.Projects.Dependencies.Dtos;
using NouAssist.Projects.Entities;
using NouAssist.Projects.Permissions;
using NouAssist.Projects.Services;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.Projects.Dependencies
{
    [Authorize(ProjectsPermissions.Dependencies.Default)]
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

        [Authorize(ProjectsPermissions.Dependencies.Create)]
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

        [Authorize(ProjectsPermissions.Dependencies.Resolve)]
        public async Task<ProjectDependencyDto> ResolveAsync(Guid id)
        {
            var dependency = await _dependencyRepository.GetAsync(id);
            
            var resolvedBy = CurrentUser.UserName ?? CurrentUser.Id?.ToString() ?? "System";
            dependency.Resolve(resolvedBy);
            
            await _dependencyRepository.UpdateAsync(dependency);

            return ObjectMapper.Map<ProjectDependency, ProjectDependencyDto>(dependency);
        }

        [Authorize(ProjectsPermissions.Dependencies.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _dependencyRepository.DeleteAsync(id);
        }
    }
}
