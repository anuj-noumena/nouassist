using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.FinancialPlanning.Entities
{
    public class CategoryAllocation : Entity
    {
        public Guid BudgetId { get; private set; }
        public Guid CategoryId { get; private set; }
        public MonetaryValue BudgetedAmount { get; private set; }
        public MonetaryValue ActualAmount { get; private set; }
        public MonetaryValue Variance => BudgetedAmount.Subtract(ActualAmount);
        public decimal PercentageUsed => 
            BudgetedAmount.Amount > 0 
                ? (ActualAmount.Amount / BudgetedAmount.Amount) * 100 
                : 0;

        protected CategoryAllocation() { }

        public CategoryAllocation(
            Guid categoryId,
            MonetaryValue budgetedAmount)
        {
            CategoryId = categoryId;
            BudgetedAmount = Check.NotNull(budgetedAmount, nameof(budgetedAmount));
            ActualAmount = MonetaryValue.Zero(budgetedAmount.Currency);
        }

        public void AddActual(MonetaryValue amount)
        {
            Check.NotNull(amount, nameof(amount));
            
            if (amount.Currency != ActualAmount.Currency)
            {
                throw new BusinessException("FPA:CurrencyMismatch", 
                    "Amount currency must match allocation currency");
            }

            ActualAmount = ActualAmount.Add(amount);
        }

        public bool IsOverBudget() => ActualAmount.Amount > BudgetedAmount.Amount;

        public override object[] GetKeys()
        {
            return new object[] { BudgetId, CategoryId };
        }
    }
}
