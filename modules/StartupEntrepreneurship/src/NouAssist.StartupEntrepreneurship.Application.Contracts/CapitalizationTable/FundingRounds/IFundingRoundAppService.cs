using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.FundingRounds.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.FundingRounds
{
    /// <summary>
    /// Application service interface for FundingRound management
    /// </summary>
    public interface IFundingRoundAppService : IApplicationService
    {
        Task<FundingRoundDto> GetAsync(Guid id);
        Task<PagedResultDto<FundingRoundDto>> GetListAsync(GetFundingRoundListDto input);
        Task<FundingRoundDto> CreateAsync(CreateFundingRoundDto input);
        Task<FundingRoundDto> UpdateAsync(Guid id, UpdateFundingRoundDto input);
        Task DeleteAsync(Guid id);
        
        // Funding round lifecycle operations
        Task<FundingRoundDto> ActivateAsync(Guid id);
        Task<FundingRoundDto> CloseAsync(Guid id, DateTime? closeDate = null);
        Task<FundingRoundDto> CancelAsync(Guid id);
        
        // Investment operations
        Task RecordInvestmentAsync(Guid id, RecordInvestmentDto input);
        
        // Query operations
        Task<decimal> GetCompletionPercentageAsync(Guid id);
        Task<decimal> GetRemainingAmountAsync(Guid id);
    }
}
