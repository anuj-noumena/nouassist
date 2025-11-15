using System;
using System.Threading.Tasks;
using NouAssist.InvestmentManagement.Portfolios.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.InvestmentManagement.Portfolios
{
    public interface IPortfolioAppService : IApplicationService
    {
        Task<PortfolioDto> GetAsync(Guid id);
        Task<PagedResultDto<PortfolioDto>> GetListAsync(GetPortfolioListDto input);
        Task<PortfolioDto> CreateAsync(CreatePortfolioDto input);
        Task<PortfolioDto> UpdateAsync(Guid id, UpdatePortfolioDto input);
        Task DeleteAsync(Guid id);
        Task<PortfolioDto> BuyHoldingAsync(Guid id, BuyHoldingDto input);
        Task<PortfolioDto> SellHoldingAsync(Guid id, SellHoldingDto input);
        Task<PortfolioDto> AddCashAsync(Guid id, AddCashDto input);
        Task<PortfolioDto> WithdrawCashAsync(Guid id, WithdrawCashDto input);
        Task<PortfolioDto> RecordDividendAsync(Guid id, RecordDividendDto input);
        Task<PortfolioDto> UpdateHoldingPricesAsync(Guid id, UpdateHoldingPricesDto input);
        Task<PerformanceMetricsDto> CalculatePerformanceAsync(Guid id, DateTime periodStart, DateTime periodEnd);
        Task<RebalancingPlanDto> CreateRebalancingPlanAsync(Guid id);
        Task<PortfolioDto> ExecuteRebalanceAsync(Guid id);
    }
}
