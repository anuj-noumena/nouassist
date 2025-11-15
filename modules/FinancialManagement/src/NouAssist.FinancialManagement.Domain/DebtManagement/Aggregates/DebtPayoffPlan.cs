using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.DebtManagement.Entities;
using NouAssist.DebtManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.DebtManagement.Aggregates
{
    public class DebtPayoffPlan : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string PlanName { get; private set; }
        public PaymentStrategy Strategy { get; private set; }
        public decimal MonthlyPaymentAmount { get; private set; }
        public DateTime StartDate { get; private set; }
        public bool IsActive { get; set; }

        private readonly List<PayoffPlanDebt> _debts = new();
        public IReadOnlyCollection<PayoffPlanDebt> Debts => _debts.AsReadOnly();

        protected DebtPayoffPlan() { /* ORM */ }

        public DebtPayoffPlan(
            Guid id,
            Guid userId,
            Guid? tenantId,
            string planName,
            PaymentStrategy strategy,
            decimal monthlyPaymentAmount)
            : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            PlanName = Check.NotNullOrWhiteSpace(planName, nameof(planName));
            Strategy = strategy;
            MonthlyPaymentAmount = Check.Positive(monthlyPaymentAmount, nameof(monthlyPaymentAmount));
            StartDate = Clock.Now;
            IsActive = true;
        }

        public void AddDebt(
            Guid debtAccountId,
            int payoffOrder,
            decimal allocatedMonthlyPayment)
        {
            if (_debts.Any(d => d.DebtAccountId == debtAccountId))
            {
                throw new BusinessException("DebtManagement:DebtAlreadyInPlan");
            }

            var planDebt = new PayoffPlanDebt(
                Id,
                debtAccountId,
                payoffOrder,
                allocatedMonthlyPayment
            );

            _debts.Add(planDebt);
        }

        public void RemoveDebt(Guid debtAccountId)
        {
            var debt = _debts.FirstOrDefault(d => d.DebtAccountId == debtAccountId);
            if (debt == null)
            {
                throw new BusinessException("DebtManagement:DebtNotInPlan");
            }

            _debts.Remove(debt);
        }

        public void UpdateMonthlyPayment(decimal newAmount)
        {
            MonthlyPaymentAmount = Check.Positive(newAmount, nameof(newAmount));
        }

        public void UpdateStrategy(PaymentStrategy newStrategy)
        {
            Strategy = newStrategy;
        }
    }
}
