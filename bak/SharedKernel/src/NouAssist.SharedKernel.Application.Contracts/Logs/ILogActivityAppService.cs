using System;
using System.Threading.Tasks;
using NouAssist.SharedKernel.Logs.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.SharedKernel.Logs
{
    public interface ILogActivityAppService : IApplicationService
    {
        Task<PagedResultDto<LogActivityDto>> GetMyActivitiesAsync(GetLogActivityListDto input);
        Task<ListResultDto<LogActivityDto>> GetByEntityAsync(string entityType, Guid entityId);
        Task<LogActivityDto> CreateAsync(CreateLogActivityDto input);
    }
}
