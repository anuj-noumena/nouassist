using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.FreelanceOperations.Upwork.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.FreelanceOperations.Upwork
{
    public interface IUpworkAppService : IApplicationService
    {
        // Upwork Credentials
        Task<UpworkCredentialsDto> GetCredentialsAsync();
        Task SetCredentialsAsync(UpworkCredentialsDto input);
        
        // Upwork Engagements
        Task<UpworkEngagementDto> GetEngagementAsync(Guid id);
        Task<PagedResultDto<UpworkEngagementDto>> GetListAsync(GetUpworkEngagementListDto input);
        Task<UpworkEngagementDto> ImportJobAsync(ImportUpworkJobDto input);
        Task<UpworkEngagementDto> UpdateContractAsync(Guid id, UpdateUpworkContractDto input);
        Task<UpworkEngagementDto> LinkToSystemAsync(Guid id, LinkEngagementToSystemDto input);
        Task<UpworkEngagementDto> UpdateStatusAsync(Guid id, string status);
        Task DeleteAsync(Guid id);
        
        // Specialized queries
        Task<List<UpworkEngagementDto>> GetActiveEngagementsAsync();
        Task<List<UpworkEngagementDto>> GetUnlinkedEngagementsAsync();
    }
}
