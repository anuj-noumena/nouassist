using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances
{
    /// <summary>
    /// Application service interface for SecurityIssuance management
    /// </summary>
    public interface ISecurityIssuanceAppService : IApplicationService
    {
        Task<SecurityIssuanceDto> GetAsync(Guid id);
        Task<PagedResultDto<SecurityIssuanceDto>> GetListAsync(GetSecurityIssuanceListDto input);
        Task<SecurityIssuanceDto> CreateStockIssuanceAsync(CreateStockIssuanceDto input);
        Task<SecurityIssuanceDto> CreateOptionIssuanceAsync(CreateOptionIssuanceDto input);
        Task<SecurityIssuanceDto> UpdateAsync(Guid id, UpdateSecurityIssuanceDto input);
        Task DeleteAsync(Guid id);
        Task<SecurityIssuanceDto> ActivateAsync(Guid id);
        Task<SecurityIssuanceDto> DeactivateAsync(Guid id);
        
        // Security issuance operations
        Task IssueSecuritiesAsync(Guid id, IssueSecuritiesDto input);
        Task CancelSecuritiesAsync(Guid id, decimal quantity);
        
        // Share class management
        Task<List<ShareClassDto>> GetShareClassesAsync(Guid id);
        Task<ShareClassDto> AddShareClassAsync(Guid id, AddShareClassDto input);
        
        // Query operations
        Task<decimal> GetAvailableSharesAsync(Guid id);
        Task<bool> IsExpiredAsync(Guid id);
    }
}
