using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Projects.Sprints.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Projects.Sprints
{
    public interface IProjectSprintAppService : IApplicationService
    {
        Task<ProjectSprintDto> GetAsync(Guid id);
        
        Task<List<ProjectSprintDto>> GetByProjectAsync(Guid projectId);
        
        Task<ProjectSprintDto> GetActiveSprintAsync(Guid projectId);
        
        Task<ProjectSprintDto> CreateAsync(CreateSprintDto input);
        
        Task<ProjectSprintDto> UpdateAsync(Guid id, UpdateSprintDto input);
        
        Task<ProjectSprintDto> StartAsync(Guid id);
        
        Task<ProjectSprintDto> CompleteAsync(Guid id, CompleteSprintDto input);
        
        Task<ProjectSprintDto> UpdateStoryPointsAsync(Guid id, UpdateSprintStoryPointsDto input);
        
        Task DeleteAsync(Guid id);
    }
}
