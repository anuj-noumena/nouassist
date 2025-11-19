using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.DebtManagement.Services
{
    public class DebtConsolidationService : DomainService
    {
        public ConsolidationAnalysis AnalyzeConsolidationOpportunity(
            List<DebtAccount> debtsToConsolidate,
            decimal consolidationLoanInterestRate,
            int consolidationLoanTermMonths)
        {
            var totalCurrentBalance = debtsToConsolidate.Sum(d => d.CurrentBalance);
            var weightedAverageRate = CalculateWeightedAverageRate(debtsToConsolidate);
            var currentTotalMinimumPayment = debtsToConsolidate.Sum(d => d.MinimumPayment);

            // Calculate total interest on current debts
            var currentTotalInterest = debtsToConsolidate.Sum(debt =>
            {
                try
                {
                    var projection = debt.CalculatePayoffProjection(debt.MinimumPayment);
                    return projection.TotalInterestToPay;
                }
                catch
                {
                    return 0m; // Skip if calculation fails
                }
            });

            // Calculate consolidated loan payment
            var monthlyRate = consolidationLoanInterestRate / 12 / 100;
            var consolidatedPayment = totalCurrentBalance * (monthlyRate / (decimal)(1 - Math.Pow(1 + (double)monthlyRate, -consolidationLoanTermMonths)));

            // Calculate total interest on consolidated loan
            var consolidatedTotalInterest = 
                (consolidatedPayment * consolidationLoanTermMonths) - totalCurrentBalance;

            var interestSavings = currentTotalInterest - consolidatedTotalInterest;
            var monthlyPaymentSavings = currentTotalMinimumPayment - consolidatedPayment;

            var isWorthwhile = interestSavings > 0 && monthlyPaymentSavings > 0;

            return new ConsolidationAnalysis(
                totalCurrentBalance,
                weightedAverageRate,
                consolidationLoanInterestRate,
                currentTotalMinimumPayment,
                consolidatedPayment,
                currentTotalInterest,
                consolidatedTotalInterest,
                interestSavings,
                monthlyPaymentSavings,
                isWorthwhile
            );
        }

        private decimal CalculateWeightedAverageRate(List<DebtAccount> debts)
        {
            var totalBalance = debts.Sum(d => d.CurrentBalance);
            if (totalBalance == 0)
            {
                return 0;
            }

            return debts.Sum(d => d.InterestRate * (d.CurrentBalance / totalBalance));
        }
    }

    public class ConsolidationAnalysis
    {
        public decimal TotalDebtToConsolidate { get; }
        public decimal CurrentWeightedAverageRate { get; }
        public decimal ConsolidationRate { get; }
        public decimal CurrentTotalMonthlyPayment { get; }
        public decimal ConsolidatedMonthlyPayment { get; }
        public decimal CurrentTotalInterest { get; }
        public decimal ConsolidatedTotalInterest { get; }
        public decimal InterestSavings { get; }
        public decimal MonthlyPaymentSavings { get; }
        public bool IsWorthwhile { get; }

        public ConsolidationAnalysis(
            decimal totalDebtToConsolidate,
            decimal currentWeightedAverageRate,
            decimal consolidationRate,
            decimal currentTotalMonthlyPayment,
            decimal consolidatedMonthlyPayment,
            decimal currentTotalInterest,
            decimal consolidatedTotalInterest,
            decimal interestSavings,
            decimal monthlyPaymentSavings,
            bool isWorthwhile)
        {
            TotalDebtToConsolidate = totalDebtToConsolidate;
            CurrentWeightedAverageRate = currentWeightedAverageRate;
            ConsolidationRate = consolidationRate;
            CurrentTotalMonthlyPayment = currentTotalMonthlyPayment;
            ConsolidatedMonthlyPayment = consolidatedMonthlyPayment;
            CurrentTotalInterest = currentTotalInterest;
            ConsolidatedTotalInterest = consolidatedTotalInterest;
            InterestSavings = interestSavings;
            MonthlyPaymentSavings = monthlyPaymentSavings;
            IsWorthwhile = isWorthwhile;
        }
    }
}
