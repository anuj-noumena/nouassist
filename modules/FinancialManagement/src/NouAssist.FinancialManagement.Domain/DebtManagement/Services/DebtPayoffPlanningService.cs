using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using NouAssist.FinancialManagement.DebtManagement.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.DebtManagement.Services
{
    public class DebtPayoffPlanningService : DomainService
    {
        public DebtPayoffPlan CreateOptimizedPayoffPlan(
            Guid userId,
            Guid? tenantId,
            string planName,
            PaymentStrategy strategy,
            decimal monthlyBudget,
            List<DebtAccount> debts)
        {
            var plan = new DebtPayoffPlan(
                GuidGenerator.Create(),
                userId,
                tenantId,
                planName,
                strategy,
                monthlyBudget
            );

            var orderedDebts = OrderDebtsByStrategy(debts, strategy);
            var totalMinimumPayments = debts.Sum(d => d.MinimumPayment);

            if (monthlyBudget < totalMinimumPayments)
            {
                throw new Volo.Abp.BusinessException("DebtManagement:InsufficientBudgetForMinimums");
            }

            var extraPayment = monthlyBudget - totalMinimumPayments;
            
            for (int i = 0; i < orderedDebts.Count; i++)
            {
                var debt = orderedDebts[i];
                var allocation = debt.MinimumPayment;

                // Allocate extra payment to first debt in priority order
                if (i == 0)
                {
                    allocation += extraPayment;
                }

                plan.AddDebt(debt.Id, i + 1, allocation);
            }

            return plan;
        }

        private List<DebtAccount> OrderDebtsByStrategy(
            List<DebtAccount> debts,
            PaymentStrategy strategy)
        {
            return strategy switch
            {
                PaymentStrategy.HighestInterestFirst => 
                    debts.OrderByDescending(d => d.InterestRate).ToList(),
                
                PaymentStrategy.LowestBalanceFirst => 
                    debts.OrderBy(d => d.CurrentBalance).ToList(),
                
                PaymentStrategy.HighestBalanceFirst => 
                    debts.OrderByDescending(d => d.CurrentBalance).ToList(),
                
                _ => debts.ToList()
            };
        }

        public PayoffAnalysis AnalyzePayoffPlan(
            DebtPayoffPlan plan,
            List<DebtAccount> debts)
        {
            var totalDebt = debts.Sum(d => d.CurrentBalance);
            var totalMinimumPayment = debts.Sum(d => d.MinimumPayment);
            
            var projections = debts.Select(debt =>
            {
                var planDebt = plan.Debts.FirstOrDefault(pd => pd.DebtAccountId == debt.Id);
                var monthlyPayment = planDebt?.AllocatedMonthlyPayment ?? debt.MinimumPayment;
                
                return new
                {
                    DebtId = debt.Id,
                    Projection = debt.CalculatePayoffProjection(monthlyPayment)
                };
            }).ToList();

            var totalMonths = projections.Max(p => p.Projection.MonthsToPayoff);
            var totalInterest = projections.Sum(p => p.Projection.TotalInterestToPay);
            var earliestPayoffDate = projections.Max(p => p.Projection.ProjectedPayoffDate);

            return new PayoffAnalysis(
                totalDebt,
                totalInterest,
                plan.MonthlyPaymentAmount,
                totalMonths,
                earliestPayoffDate
            );
        }
    }

    public class PayoffAnalysis
    {
        public decimal TotalDebt { get; }
        public decimal TotalInterest { get; }
        public decimal MonthlyPayment { get; }
        public int MonthsToPayoff { get; }
        public DateTime ProjectedPayoffDate { get; }

        public PayoffAnalysis(
            decimal totalDebt,
            decimal totalInterest,
            decimal monthlyPayment,
            int monthsToPayoff,
            DateTime projectedPayoffDate)
        {
            TotalDebt = totalDebt;
            TotalInterest = totalInterest;
            MonthlyPayment = monthlyPayment;
            MonthsToPayoff = monthsToPayoff;
            ProjectedPayoffDate = projectedPayoffDate;
        }
    }
}
