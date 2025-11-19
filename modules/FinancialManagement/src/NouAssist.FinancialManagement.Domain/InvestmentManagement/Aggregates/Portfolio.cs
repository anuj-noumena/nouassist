using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.InvestmentManagement.Entities;
using NouAssist.FinancialManagement.InvestmentManagement.Enums;
using NouAssist.FinancialManagement.InvestmentManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.InvestmentManagement.Aggregates
{
    public class Portfolio : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; set; }
        public RiskTolerance RiskTolerance { get; private set; }
        public PortfolioStatus Status { get; private set; }

        public decimal TotalValue { get; private set; }
        public decimal TotalCostBasis { get; private set; }
        public decimal TotalGainLoss { get; private set; }
        public decimal TotalGainLossPercentage { get; private set; }
        public decimal CashBalance { get; private set; }

        public DateTime? LastRebalanceDate { get; private set; }

        private readonly List<Holding> _holdings = new();
        public IReadOnlyCollection<Holding> Holdings => _holdings.AsReadOnly();

        private readonly List<InvestmentTransaction> _transactions = new();
        public IReadOnlyCollection<InvestmentTransaction> Transactions => _transactions.AsReadOnly();

        protected Portfolio() { /* ORM */ }

        public Portfolio(
            Guid id,
            Guid userId,
            Guid? tenantId,
            string name,
            RiskTolerance riskTolerance)
            : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            RiskTolerance = riskTolerance;
            Status = PortfolioStatus.Active;
            CashBalance = 0;
        }

        public void BuyHolding(
            string symbol,
            string name,
            InvestmentType type,
            AssetClass assetClass,
            decimal quantity,
            decimal price,
            decimal fees,
            DateTime transactionDate)
        {
            if (Status != PortfolioStatus.Active)
            {
                throw new BusinessException("InvestmentManagement:PortfolioNotActive");
            }

            var totalCost = (quantity * price) + fees;
            if (totalCost > CashBalance)
            {
                throw new BusinessException("InvestmentManagement:InsufficientCash");
            }

            var existingHolding = _holdings.FirstOrDefault(h => h.Symbol == symbol);
            if (existingHolding != null)
            {
                existingHolding.AddQuantity(quantity, price);
            }
            else
            {
                var newHolding = new Holding(
                    Id,
                    symbol,
                    name,
                    type,
                    assetClass,
                    quantity,
                    price,
                    price
                );
                _holdings.Add(newHolding);
            }

            var transaction = new InvestmentTransaction(
                Guid.NewGuid(),
                Id,
                TransactionType.Buy,
                symbol,
                quantity,
                price,
                fees,
                transactionDate
            );
            _transactions.Add(transaction);

            CashBalance -= totalCost;
            RecalculatePortfolioMetrics();
        }

        public void SellHolding(
            string symbol,
            decimal quantity,
            decimal price,
            decimal fees,
            DateTime transactionDate)
        {
            if (Status != PortfolioStatus.Active)
            {
                throw new BusinessException("InvestmentManagement:PortfolioNotActive");
            }

            var holding = _holdings.FirstOrDefault(h => h.Symbol == symbol);
            if (holding == null)
            {
                throw new BusinessException("InvestmentManagement:HoldingNotFound");
            }

            if (quantity > holding.Quantity)
            {
                throw new BusinessException("InvestmentManagement:InsufficientQuantity");
            }

            holding.ReduceQuantity(quantity);
            
            if (holding.Quantity == 0)
            {
                _holdings.Remove(holding);
            }

            var transaction = new InvestmentTransaction(
                Guid.NewGuid(),
                Id,
                TransactionType.Sell,
                symbol,
                quantity,
                price,
                fees,
                transactionDate
            );
            _transactions.Add(transaction);

            var proceeds = (quantity * price) - fees;
            CashBalance += proceeds;
            RecalculatePortfolioMetrics();
        }

        public void AddCash(decimal amount, DateTime transactionDate)
        {
            if (amount <= 0)
            {
                throw new BusinessException("InvestmentManagement:AmountMustBePositive");
            }

            var transaction = new InvestmentTransaction(
                Guid.NewGuid(),
                Id,
                TransactionType.Deposit,
                "CASH",
                1,
                amount,
                0,
                transactionDate
            );
            _transactions.Add(transaction);

            CashBalance += amount;
            RecalculatePortfolioMetrics();
        }

        public void WithdrawCash(decimal amount, DateTime transactionDate)
        {
            if (amount <= 0)
            {
                throw new BusinessException("InvestmentManagement:AmountMustBePositive");
            }

            if (amount > CashBalance)
            {
                throw new BusinessException("InvestmentManagement:InsufficientCashBalance");
            }

            var transaction = new InvestmentTransaction(
                Guid.NewGuid(),
                Id,
                TransactionType.Withdrawal,
                "CASH",
                1,
                amount,
                0,
                transactionDate
            );
            _transactions.Add(transaction);

            CashBalance -= amount;
            RecalculatePortfolioMetrics();
        }

        public void RecordDividend(
            string symbol,
            decimal amount,
            DateTime transactionDate)
        {
            var transaction = new InvestmentTransaction(
                Guid.NewGuid(),
                Id,
                TransactionType.Dividend,
                symbol,
                1,
                amount,
                0,
                transactionDate
            );
            _transactions.Add(transaction);

            CashBalance += amount;
        }

        public void UpdateHoldingPrices(Dictionary<string, decimal> symbolPrices)
        {
            foreach (var holding in _holdings)
            {
                if (symbolPrices.ContainsKey(holding.Symbol))
                {
                    holding.UpdatePrice(symbolPrices[holding.Symbol]);
                }
            }

            RecalculatePortfolioMetrics();
        }

        public void MarkAsRebalanced()
        {
            LastRebalanceDate = DateTime.UtcNow;
        }

        public void RecalculatePortfolioMetrics()
        {
            TotalValue = _holdings.Sum(h => h.MarketValue) + CashBalance;
            TotalCostBasis = _holdings.Sum(h => h.Quantity * h.AverageCostBasis);
            TotalGainLoss = _holdings.Sum(h => h.GetUnrealizedGainLoss());
            TotalGainLossPercentage = TotalCostBasis > 0 
                ? (TotalGainLoss / TotalCostBasis) * 100 
                : 0;
        }

        public PortfolioAllocation GetCurrentAllocation()
        {
            var allocations = new Dictionary<string, decimal>();
            
            if (TotalValue == 0)
            {
                return new PortfolioAllocation(allocations);
            }

            foreach (var assetClass in Enum.GetValues<AssetClass>())
            {
                var classValue = _holdings
                    .Where(h => h.AssetClass == assetClass)
                    .Sum(h => h.MarketValue);
                
                if (classValue > 0)
                {
                    allocations[assetClass.ToString()] = (classValue / TotalValue) * 100;
                }
            }

            // Add cash allocation
            if (CashBalance > 0)
            {
                allocations["Cash"] = (CashBalance / TotalValue) * 100;
            }

            return new PortfolioAllocation(allocations);
        }

        public Dictionary<string, decimal> GetHoldingWeights()
        {
            var weights = new Dictionary<string, decimal>();
            
            if (TotalValue == 0)
            {
                return weights;
            }

            foreach (var holding in _holdings)
            {
                weights[holding.Symbol] = (holding.MarketValue / TotalValue) * 100;
            }

            return weights;
        }
    }
}
