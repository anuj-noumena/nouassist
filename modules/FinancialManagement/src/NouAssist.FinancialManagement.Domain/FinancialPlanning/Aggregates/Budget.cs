using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using NouAssist.FinancialManagement.FinancialPlanning.Events;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Aggregates
{
    public class Budget : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public BudgetType Type { get; private set; }
        public DateTime PeriodStart { get; private set; }
        public DateTime PeriodEnd { get; private set; }
        public BudgetStatus Status { get; private set; }
        public MonetaryValue TotalBudget { get; private set; }
        public MonetaryValue ActualSpent { get; private set; }
        public decimal VariancePercentage => CalculateVariancePercentage();

        private readonly List<CategoryAllocation> _allocations = new();
        public IReadOnlyCollection<CategoryAllocation> Allocations => _allocations.AsReadOnly();

        protected Budget() { } // For ORM

        public Budget(
            Guid id,
            Guid? tenantId,
            Guid userId,
            string name,
            BudgetType type,
            DateTime periodStart,
            DateTime periodEnd,
            MonetaryValue totalBudget)
            : base(id)
        {
            if (periodEnd <= periodStart)
                throw new BusinessException("FinancialPlanning:InvalidPeriod",
                    "Period end must be after period start");

            TenantId = tenantId;
            UserId = userId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), 200);
            Type = type;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            Status = BudgetStatus.Draft;
            TotalBudget = Check.NotNull(totalBudget, nameof(totalBudget));
            ActualSpent = new MonetaryValue(0, totalBudget.Currency);
        }

        public void AddAllocation(CategoryAllocation allocation)
        {
            if (Status != BudgetStatus.Draft)
                throw new BusinessException("FinancialPlanning:CannotModifyActiveBudget",
                    "Cannot modify allocations of active budget");

            var existingAllocation = _allocations.FirstOrDefault(a => a.CategoryId == allocation.CategoryId);
            if (existingAllocation != null)
                throw new BusinessException("FinancialPlanning:DuplicateCategory",
                    "Category already has an allocation");

            _allocations.Add(allocation);
            RecalculateTotalBudget();
        }

        public void UpdateAllocation(Guid categoryId, MonetaryValue newAmount, string justification)
        {
            var allocation = _allocations.FirstOrDefault(a => a.CategoryId == categoryId);
            if (allocation == null)
                throw new BusinessException("FinancialPlanning:AllocationNotFound",
                    "Allocation for category not found");

            allocation.UpdateBudgetedAmount(newAmount);
            RecalculateTotalBudget();
        }

        public void TrackActual(Guid categoryId, MonetaryValue amount, DateTime transactionDate)
        {
            if (Status != BudgetStatus.Active)
                throw new BusinessException("FinancialPlanning:BudgetNotActive",
                    "Can only track actuals for active budgets");

            if (transactionDate < PeriodStart || transactionDate > PeriodEnd)
                throw new BusinessException("FinancialPlanning:TransactionOutsidePeriod",
                    "Transaction date is outside budget period");

            var allocation = _allocations.FirstOrDefault(a => a.CategoryId == categoryId);
            if (allocation == null)
                throw new BusinessException("FinancialPlanning:AllocationNotFound",
                    "No allocation found for this category");

            allocation.AddActualAmount(amount);
            ActualSpent = ActualSpent.Add(amount);

            // Check for variance alerts
            var variancePercentage = allocation.CalculateVariancePercentage();
            if (variancePercentage > 90)
            {
                AddDistributedEvent(new BudgetVarianceExceededEto
                {
                    IdempotencyKey = Guid.NewGuid().ToString(),
                    BudgetId = Id,
                    CategoryId = categoryId,
                    BudgetedAmount = allocation.BudgetedAmount,
                    ActualAmount = allocation.ActualAmount,
                    VariancePercentage = variancePercentage,
                    DetectedDate = DateTime.UtcNow,
                    TenantId = TenantId
                });
            }
        }

        public void Activate()
        {
            if (Status != BudgetStatus.Draft)
                throw new BusinessException("FinancialPlanning:BudgetNotDraft",
                    "Only draft budgets can be activated");

            if (!_allocations.Any())
                throw new BusinessException("FinancialPlanning:NoAllocations",
                    "Budget must have at least one allocation");

            Status = BudgetStatus.Active;
        }

        public void Close()
        {
            if (Status != BudgetStatus.Active)
                throw new BusinessException("FinancialPlanning:BudgetNotActive",
                    "Only active budgets can be closed");

            Status = BudgetStatus.Closed;
        }

        public void Archive()
        {
            if (Status != BudgetStatus.Closed)
                throw new BusinessException("FinancialPlanning:BudgetNotClosed",
                    "Only closed budgets can be archived");

            Status = BudgetStatus.Archived;
        }

        private void RecalculateTotalBudget()
        {
            if (_allocations.Any())
            {
                var total = _allocations.Sum(a => a.BudgetedAmount.Amount);
                var currency = _allocations.First().BudgetedAmount.Currency;
                TotalBudget = new MonetaryValue(total, currency);
            }
        }

        private decimal CalculateVariancePercentage()
        {
            if (TotalBudget.IsZero) return 0;
            return (ActualSpent.Amount / TotalBudget.Amount) * 100;
        }
    }
}
