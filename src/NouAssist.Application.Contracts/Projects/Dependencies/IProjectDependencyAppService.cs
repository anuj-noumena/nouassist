using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Projects.Dependencies.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Projects.Dependencies
{
    public interface IProjectDependencyAppService : IApplicationService
    {
        Task<ProjectDependencyDto> GetAsync(Guid id);
        
        Task<List<ProjectDependencyDto>> GetByProjectAsync(Guid projectId);
        
        Task<List<ProjectDependencyDto>> GetBlockingDependenciesAsync(Guid projectId);
        
        Task<ProjectDependencyDto> CreateAsync(CreateDependencyDto input);
        
        Task<ProjectDependencyDto> ResolveAsync(Guid id);
        
        Task DeleteAsync(Guid id);
    }
}
