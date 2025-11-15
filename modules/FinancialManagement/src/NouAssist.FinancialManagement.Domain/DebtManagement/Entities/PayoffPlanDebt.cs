using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.DebtManagement.Entities
{
    public class PayoffPlanDebt : Entity
    {
        public Guid PayoffPlanId { get; private set; }
        public Guid DebtAccountId { get; private set; }
        public int PayoffOrder { get; private set; }
        public decimal AllocatedMonthlyPayment { get; private set; }
        public bool IsIncluded { get; set; }

        protected PayoffPlanDebt() { /* ORM */ }

        public PayoffPlanDebt(
            Guid payoffPlanId,
            Guid debtAccountId,
            int payoffOrder,
            decimal allocatedMonthlyPayment)
        {
            PayoffPlanId = payoffPlanId;
            DebtAccountId = debtAccountId;
            PayoffOrder = Check.Positive(payoffOrder, nameof(payoffOrder));
            AllocatedMonthlyPayment = Check.Positive(allocatedMonthlyPayment, nameof(allocatedMonthlyPayment));
            IsIncluded = true;
        }

        public void UpdateAllocation(decimal newAmount)
        {
            AllocatedMonthlyPayment = Check.Positive(newAmount, nameof(newAmount));
        }

        public void UpdatePayoffOrder(int newOrder)
        {
            PayoffOrder = Check.Positive(newOrder, nameof(newOrder));
        }

        public override object[] GetKeys() => new object[] { PayoffPlanId, DebtAccountId };
    }
}
