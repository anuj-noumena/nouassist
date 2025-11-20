using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty
{
    public interface IIPAssetAppService : IApplicationService
    {
        Task<IPAssetDto> GetAsync(Guid id);
        
        Task<PagedResultDto<IPAssetListDto>> GetListAsync(GetIPAssetListDto input);
        
        Task<IPAssetDto> CreateAsync(CreateIPAssetDto input);
        
        Task<IPAssetDto> UpdateAsync(Guid id, UpdateIPAssetDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<IPAssetDto> UpdateStatusAsync(Guid id, FilingStatusType newStatus, string notes = null);
        
        Task SetValuationAsync(Guid id, IPValuationDto input);
        
        Task<IPDeadlineDto> AddDeadlineAsync(Guid ipId, CreateIPDeadlineDto input);
        
        Task RemoveDeadlineAsync(Guid ipId, Guid deadlineId);
        
        Task<IPDeadlineDto> CompleteDeadlineAsync(Guid ipId, Guid deadlineId);
        
        Task<IPAssetDto> RenewAsync(Guid id, DateTime renewalDate, DateTime newExpirationDate, string notes = null);
        
        Task<List<IPAssetListDto>> GetApproachingDeadlinesAsync(int daysThreshold = 30);
    }
}
