using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
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

        private readonly List<CategoryAllocation> _allocations = new();
        public IReadOnlyCollection<CategoryAllocation> Allocations => _allocations.AsReadOnly();

        protected Budget() { }

        public Budget(
            Guid id,
            Guid? tenantId,
            Guid userId,
            string name,
            BudgetType type,
            DateTime periodStart,
            DateTime periodEnd,
            string currency) : base(id)
        {
            if (periodEnd <= periodStart)
            {
                throw new BusinessException("FPA:InvalidPeriod", 
                    "Period end must be after period start");
            }

            TenantId = tenantId;
            UserId = userId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Type = type;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            Status = BudgetStatus.Draft;
            TotalBudget = MonetaryValue.Zero(currency);
            ActualSpent = MonetaryValue.Zero(currency);
        }

        public void AddAllocation(CategoryAllocation allocation)
        {
            if (Status != BudgetStatus.Draft)
            {
                throw new BusinessException("FPA:CannotModifyActiveBudget", 
                    "Cannot modify active budget");
            }

            _allocations.Add(allocation);
            RecalculateTotalBudget();
        }

        public void TrackActual(Guid categoryId, MonetaryValue amount)
        {
            var allocation = _allocations.FirstOrDefault(a => a.CategoryId == categoryId);
            if (allocation == null)
            {
                throw new BusinessException("FPA:CategoryNotFound", 
                    "Category not found in budget");
            }

            allocation.AddActual(amount);
            ActualSpent = ActualSpent.Add(amount);
        }

        public void Activate()
        {
            if (Status != BudgetStatus.Draft)
            {
                throw new BusinessException("FPA:AlreadyActive", 
                    "Budget is already active");
            }

            if (!_allocations.Any())
            {
                throw new BusinessException("FPA:NoAllocations", 
                    "Budget must have at least one allocation");
            }

            Status = BudgetStatus.Active;
        }

        public void Close()
        {
            if (Status != BudgetStatus.Active)
            {
                throw new BusinessException("FPA:NotActive", 
                    "Only active budgets can be closed");
            }

            Status = BudgetStatus.Closed;
        }

        public decimal GetVariancePercentage()
        {
            if (TotalBudget.Amount == 0)
            {
                return 0;
            }

            return (ActualSpent.Amount / TotalBudget.Amount) * 100;
        }

        private void RecalculateTotalBudget()
        {
            var currency = _allocations.FirstOrDefault()?.BudgetedAmount.Currency ?? "USD";
            var total = _allocations.Sum(a => a.BudgetedAmount.Amount);
            TotalBudget = new MonetaryValue(total, currency);
        }
    }
}
