using System;
using System.Threading.Tasks;
using NouAssist.Projects.Projects.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Projects.Projects
{
    public interface IProjectAppService : IApplicationService
    {
        Task<ProjectDto> GetAsync(Guid id);
        
        Task<PagedResultDto<ProjectDto>> GetListAsync(GetProjectListDto input);
        
        Task<ProjectDto> CreateAsync(CreateProjectDto input);
        
        Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<ProjectDto> StartAsync(Guid id);
        
        Task<ProjectDto> CompleteAsync(Guid id);
        
        Task<ProjectDto> CancelAsync(Guid id);
        
        Task<ProjectDto> PauseAsync(Guid id);
        
        Task<ProjectDto> ArchiveAsync(Guid id);
        
        Task<ProjectDto> UpdateProgressAsync(Guid id, UpdateProjectProgressDto input);
        
        Task<ProjectDto> UpdatePriorityAsync(Guid id, UpdateProjectPriorityDto input);
    }
}
