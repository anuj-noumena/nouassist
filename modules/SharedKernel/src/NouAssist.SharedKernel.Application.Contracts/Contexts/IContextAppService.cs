using System;
using System.Threading.Tasks;
using NouAssist.Core.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Core.Contexts
{
    public interface IContextAppService : IApplicationService
    {
        Task<ContextDto> GetAsync(Guid id);
        Task<ListResultDto<ContextDto>> GetMyActiveContextsAsync();
        Task<ListResultDto<ContextDto>> GetByLocationAsync(string location);
        Task<ContextDto> CreateAsync(CreateContextDto input);
        Task<ContextDto> UpdateAsync(Guid id, UpdateContextDto input);
        Task DeleteAsync(Guid id);
        Task<ContextDto> ArchiveAsync(Guid id);
        Task<ContextDto> ReactivateAsync(Guid id);
        Task<ContextDto> RecordUsageAsync(Guid id);
    }
}
