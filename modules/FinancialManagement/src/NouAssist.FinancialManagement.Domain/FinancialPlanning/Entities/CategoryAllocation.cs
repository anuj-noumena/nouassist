using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.FinancialPlanning.Entities
{
    public class CategoryAllocation : Entity
    {
        public Guid BudgetId { get; private set; }
        public Guid CategoryId { get; private set; }
        public MonetaryValue BudgetedAmount { get; private set; }
        public MonetaryValue ActualAmount { get; private set; }
        public MonetaryValue Variance => CalculateVariance();
        public decimal PercentageOfTotal { get; private set; }

        protected CategoryAllocation() { }

        public CategoryAllocation(
            Guid budgetId,
            Guid categoryId,
            MonetaryValue budgetedAmount)
        {
            BudgetId = budgetId;
            CategoryId = categoryId;
            BudgetedAmount = budgetedAmount;
            ActualAmount = new MonetaryValue(0, budgetedAmount.Currency);
        }

        public void UpdateBudgetedAmount(MonetaryValue newAmount)
        {
            BudgetedAmount = newAmount;
        }

        public void AddActualAmount(MonetaryValue amount)
        {
            ActualAmount = ActualAmount.Add(amount);
        }

        public void UpdatePercentageOfTotal(decimal percentage)
        {
            PercentageOfTotal = percentage;
        }

        public decimal CalculateVariancePercentage()
        {
            if (BudgetedAmount.IsZero) return 0;
            return (ActualAmount.Amount / BudgetedAmount.Amount) * 100;
        }

        private MonetaryValue CalculateVariance()
        {
            return BudgetedAmount.Subtract(ActualAmount);
        }

        public override object[] GetKeys()
        {
            return new object[] { BudgetId, CategoryId };
        }
    }
}
