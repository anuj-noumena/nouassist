using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using NouAssist.FinancialManagement.InvestmentManagement.Data;
using NouAssist.FinancialManagement.InvestmentManagement.Entities;
using NouAssist.FinancialManagement.InvestmentManagement.Permissions;
using NouAssist.FinancialManagement.InvestmentManagement.Portfolios.Dtos;
using NouAssist.FinancialManagement.InvestmentManagement.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.InvestmentManagement.Portfolios
{
    [Authorize(InvestmentManagementPermissions.Portfolios.Default)]
    public class PortfolioAppService : ApplicationService, IPortfolioAppService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly PortfolioRebalancingService _rebalancingService;
        private readonly PerformanceCalculationService _performanceService;

        public PortfolioAppService(
            IPortfolioRepository portfolioRepository,
            PortfolioRebalancingService rebalancingService,
            PerformanceCalculationService performanceService)
        {
            _portfolioRepository = portfolioRepository;
            _rebalancingService = rebalancingService;
            _performanceService = performanceService;
        }

        public async Task<PortfolioDto> GetAsync(Guid id)
        {
            var portfolio = await _portfolioRepository.GetWithDetailsAsync(id);
            var dto = ObjectMapper.Map<Portfolio, PortfolioDto>(portfolio);
            
            // Get recent transactions (last 10)
            dto.RecentTransactions = portfolio.Transactions
                .OrderByDescending(t => t.TransactionDate)
                .Take(10)
                .Select(t => ObjectMapper.Map<InvestmentTransaction, InvestmentTransactionDto>(t))
                .ToList();

            return dto;
        }

        public async Task<PagedResultDto<PortfolioDto>> GetListAsync(GetPortfolioListDto input)
        {
            var userId = CurrentUser.GetId();
            var portfolios = await _portfolioRepository.GetUserPortfoliosAsync(userId);

            if (input.Status.HasValue)
            {
                portfolios = portfolios.Where(p => p.Status == input.Status.Value).ToList();
            }

            var totalCount = portfolios.Count;
            var pagedPortfolios = portfolios
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<PortfolioDto>(
                totalCount,
                ObjectMapper.Map<List<Portfolio>, List<PortfolioDto>>(pagedPortfolios)
            );
        }

        [Authorize(InvestmentManagementPermissions.Portfolios.Create)]
        public async Task<PortfolioDto> CreateAsync(CreatePortfolioDto input)
        {
            var userId = CurrentUser.GetId();

            var portfolio = new Portfolio(
                GuidGenerator.Create(),
                userId,
                CurrentTenant.Id,
                input.Name,
                input.RiskTolerance
            );

            if (!string.IsNullOrWhiteSpace(input.Description))
            {
                portfolio.Description = input.Description;
            }

            if (input.InitialCashDeposit > 0)
            {
                portfolio.AddCash(input.InitialCashDeposit, Clock.Now);
            }

            await _portfolioRepository.InsertAsync(portfolio);

            return ObjectMapper.Map<Portfolio, PortfolioDto>(portfolio);
        }

        [Authorize(InvestmentManagementPermissions.Portfolios.Update)]
        public async Task<PortfolioDto> UpdateAsync(Guid id, UpdatePortfolioDto input)
        {
            var portfolio = await _portfolioRepository.GetAsync(id);

            // Update properties through reflection or add methods to aggregate
            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                // portfolio.UpdateName(input.Name);
            }

            if (!string.IsNullOrWhiteSpace(input.Description))
            {
                portfolio.Description = input.Description;
            }

            await _portfolioRepository.UpdateAsync(portfolio);

            return ObjectMapper.Map<Portfolio, PortfolioDto>(portfolio);
        }

        [Authorize(InvestmentManagementPermissions.Portfolios.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _portfolioRepository.DeleteAsync(id);
        }

        [Authorize(InvestmentManagementPermissions.Portfolios.Trade)]
        public async Task<PortfolioDto> BuyHoldingAsync(Guid id, BuyHoldingDto input)
        {
            var portfolio = await _portfolioRepository.GetWithDetailsAsync(id);

            portfolio.BuyHolding(
                input.Symbol,
                input.Name,
                input.Type,
                input.AssetClass,
                input.Quantity,
                input.Price,
                input.Fees,
                input.TransactionDate
            );

            await _portfolioRepository.UpdateAsync(portfolio);

            return ObjectMapper.Map<Portfolio, PortfolioDto>(portfolio);
        }

        [Authorize(InvestmentManagementPermissions.Portfolios.Trade)]
        public async Task<PortfolioDto> SellHoldingAsync(Guid id, SellHoldingDto input)
        {
            var portfolio = await _portfolioRepository.GetWithDetailsAsync(id);

            portfolio.SellHolding(
                input.Symbol,
                input.Quantity,
                input.Price,
                input.Fees,
                input.TransactionDate
            );

            await _portfolioRepository.UpdateAsync(portfolio);

            return ObjectMapper.Map<Portfolio, PortfolioDto>(portfolio);
        }

        [Authorize(InvestmentManagementPermissions.Portfolios.Trade)]
        public async Task<PortfolioDto> AddCashAsync(Guid id, AddCashDto input)
        {
            var portfolio = await _portfolioRepository.GetAsync(id);

            portfolio.AddCash(input.Amount, input.TransactionDate);

            await _portfolioRepository.UpdateAsync(portfolio);

            return ObjectMapper.Map<Portfolio, PortfolioDto>(portfolio);
        }

        [Authorize(InvestmentManagementPermissions.Portfolios.Trade)]
        public async Task<PortfolioDto> WithdrawCashAsync(Guid id, WithdrawCashDto input)
        {
            var portfolio = await _portfolioRepository.GetAsync(id);

            portfolio.WithdrawCash(input.Amount, input.TransactionDate);

            await _portfolioRepository.UpdateAsync(portfolio);

            return ObjectMapper.Map<Portfolio, PortfolioDto>(portfolio);
        }

        [Authorize(InvestmentManagementPermissions.Portfolios.Trade)]
        public async Task<PortfolioDto> RecordDividendAsync(Guid id, RecordDividendDto input)
        {
            var portfolio = await _portfolioRepository.GetAsync(id);

            portfolio.RecordDividend(input.Symbol, input.Amount, input.TransactionDate);

            await _portfolioRepository.UpdateAsync(portfolio);

            return ObjectMapper.Map<Portfolio, PortfolioDto>(portfolio);
        }

        [Authorize(InvestmentManagementPermissions.Portfolios.Update)]
        public async Task<PortfolioDto> UpdateHoldingPricesAsync(Guid id, UpdateHoldingPricesDto input)
        {
            var portfolio = await _portfolioRepository.GetWithDetailsAsync(id);

            portfolio.UpdateHoldingPrices(input.SymbolPrices);

            await _portfolioRepository.UpdateAsync(portfolio);

            return ObjectMapper.Map<Portfolio, PortfolioDto>(portfolio);
        }

        public async Task<PerformanceMetricsDto> CalculatePerformanceAsync(
            Guid id,
            DateTime periodStart,
            DateTime periodEnd)
        {
            var portfolio = await _portfolioRepository.GetWithDetailsAsync(id);

            var metrics = _performanceService.CalculatePerformance(portfolio, periodStart, periodEnd);

            return new PerformanceMetricsDto
            {
                TotalReturn = metrics.TotalReturn,
                TotalReturnPercentage = metrics.TotalReturnPercentage,
                AnnualizedReturn = metrics.AnnualizedReturn,
                VolatilityStdDev = metrics.VolatilityStdDev,
                SharpeRatio = metrics.SharpeRatio,
                MaxDrawdown = metrics.MaxDrawdown,
                PeriodStart = metrics.PeriodStart,
                PeriodEnd = metrics.PeriodEnd
            };
        }

        [Authorize(InvestmentManagementPermissions.Portfolios.Rebalance)]
        public async Task<RebalancingPlanDto> CreateRebalancingPlanAsync(Guid id)
        {
            var portfolio = await _portfolioRepository.GetWithDetailsAsync(id);
            var targetAllocation = _rebalancingService.GetTargetAllocationForRiskTolerance(portfolio.RiskTolerance);

            var plan = _rebalancingService.CreateRebalancingPlan(portfolio, targetAllocation);

            return new RebalancingPlanDto
            {
                PortfolioId = plan.PortfolioId,
                NeedsRebalancing = plan.NeedsRebalancing,
                Message = plan.Message,
                Trades = plan.Trades.Select(t => new RebalancingTradeDto
                {
                    AssetClass = t.AssetClass,
                    Action = t.Action,
                    Amount = t.Amount,
                    AllocationDifference = t.AllocationDifference
                }).ToList()
            };
        }

        [Authorize(InvestmentManagementPermissions.Portfolios.Rebalance)]
        public async Task<PortfolioDto> ExecuteRebalanceAsync(Guid id)
        {
            var portfolio = await _portfolioRepository.GetWithDetailsAsync(id);

            // Mark as rebalanced
            portfolio.MarkAsRebalanced();

            await _portfolioRepository.UpdateAsync(portfolio);

            return ObjectMapper.Map<Portfolio, PortfolioDto>(portfolio);
        }
    }
}
