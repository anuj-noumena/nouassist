using System;
using System.Collections.Generic;
using NouAssist.InvestmentManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.InvestmentManagement.Portfolios.Dtos
{
    public class PortfolioDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RiskTolerance RiskTolerance { get; set; }
        public PortfolioStatus Status { get; set; }

        public decimal TotalValue { get; set; }
        public decimal TotalCostBasis { get; set; }
        public decimal TotalGainLoss { get; set; }
        public decimal TotalGainLossPercentage { get; set; }
        public decimal CashBalance { get; set; }

        public DateTime? LastRebalanceDate { get; set; }

        public List<HoldingDto> Holdings { get; set; }
        public List<InvestmentTransactionDto> RecentTransactions { get; set; }
        public PortfolioAllocationDto CurrentAllocation { get; set; }
    }

    public class HoldingDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public InvestmentType Type { get; set; }
        public AssetClass AssetClass { get; set; }
        public decimal Quantity { get; set; }
        public decimal AverageCostBasis { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal MarketValue { get; set; }
        public decimal UnrealizedGainLoss { get; set; }
        public decimal UnrealizedGainLossPercentage { get; set; }
        public DateTime LastPriceUpdate { get; set; }
    }

    public class InvestmentTransactionDto
    {
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Fees { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Notes { get; set; }
    }

    public class PortfolioAllocationDto
    {
        public Dictionary<string, decimal> AssetClassPercentages { get; set; }
    }

    public class CreatePortfolioDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public RiskTolerance RiskTolerance { get; set; }
        public decimal InitialCashDeposit { get; set; }
    }

    public class UpdatePortfolioDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public RiskTolerance? RiskTolerance { get; set; }
    }

    public class BuyHoldingDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public InvestmentType Type { get; set; }
        public AssetClass AssetClass { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Fees { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class SellHoldingDto
    {
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Fees { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class AddCashDto
    {
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class WithdrawCashDto
    {
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class RecordDividendDto
    {
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class UpdateHoldingPricesDto
    {
        public Dictionary<string, decimal> SymbolPrices { get; set; }
    }

    public class GetPortfolioListDto : PagedAndSortedResultRequestDto
    {
        public PortfolioStatus? Status { get; set; }
    }

    public class PerformanceMetricsDto
    {
        public decimal TotalReturn { get; set; }
        public decimal TotalReturnPercentage { get; set; }
        public decimal AnnualizedReturn { get; set; }
        public decimal VolatilityStdDev { get; set; }
        public decimal SharpeRatio { get; set; }
        public decimal MaxDrawdown { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }

    public class RebalancingPlanDto
    {
        public Guid PortfolioId { get; set; }
        public List<RebalancingTradeDto> Trades { get; set; }
        public bool NeedsRebalancing { get; set; }
        public string Message { get; set; }
    }

    public class RebalancingTradeDto
    {
        public string AssetClass { get; set; }
        public string Action { get; set; }
        public decimal Amount { get; set; }
        public decimal AllocationDifference { get; set; }
    }
}
