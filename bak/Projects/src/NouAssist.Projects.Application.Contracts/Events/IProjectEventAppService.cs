using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Projects.Events.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Projects.Events
{
    public interface IProjectEventAppService : IApplicationService
    {
        Task<ProjectEventDto> GetAsync(Guid id);
        
        Task<List<ProjectEventDto>> GetByProjectAsync(Guid projectId);
        
        Task<List<ProjectEventDto>> GetUpcomingEventsAsync(Guid projectId, int days = 30);
        
        Task<ProjectEventDto> CreateAsync(CreateEventDto input);
        
        Task<ProjectEventDto> MarkAsCompletedAsync(Guid id);
        
        Task DeleteAsync(Guid id);
    }
}
