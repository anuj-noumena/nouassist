using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.TaxManagement.Aggregates;
using NouAssist.TaxManagement.Enums;
using NouAssist.TaxManagement.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.TaxManagement.Services
{
    public class TaxCalculationService : DomainService
    {
        // 2024 Tax Brackets (example - should be configurable)
        private readonly Dictionary<TaxFilingStatus, List<TaxBracket>> _taxBrackets = new()
        {
            {
                TaxFilingStatus.Single,
                new List<TaxBracket>
                {
                    new TaxBracket(0, 11000, 0.10m, 0),
                    new TaxBracket(11000, 44725, 0.12m, 1100),
                    new TaxBracket(44725, 95375, 0.22m, 5147),
                    new TaxBracket(95375, 182100, 0.24m, 16290),
                    new TaxBracket(182100, 231250, 0.32m, 37104),
                    new TaxBracket(231250, 578125, 0.35m, 52832),
                    new TaxBracket(578125, decimal.MaxValue, 0.37m, 174238.25m)
                }
            },
            {
                TaxFilingStatus.MarriedFilingJointly,
                new List<TaxBracket>
                {
                    new TaxBracket(0, 22000, 0.10m, 0),
                    new TaxBracket(22000, 89050, 0.12m, 2200),
                    new TaxBracket(89050, 190750, 0.22m, 10294),
                    new TaxBracket(190750, 364200, 0.24m, 32580),
                    new TaxBracket(364200, 462500, 0.32m, 74208),
                    new TaxBracket(462500, 693750, 0.35m, 105664),
                    new TaxBracket(693750, decimal.MaxValue, 0.37m, 186601.50m)
                }
            }
        };

        private readonly Dictionary<TaxFilingStatus, decimal> _standardDeductions = new()
        {
            { TaxFilingStatus.Single, 13850m },
            { TaxFilingStatus.MarriedFilingJointly, 27700m },
            { TaxFilingStatus.MarriedFilingSeparately, 13850m },
            { TaxFilingStatus.HeadOfHousehold, 20800m },
            { TaxFilingStatus.QualifyingWidower, 27700m }
        };

        public TaxCalculation CalculateTaxLiability(
            TaxReturn taxReturn,
            decimal totalDeductions)
        {
            var grossIncome = taxReturn.GrossIncome;
            
            // Calculate AGI (simplified - would include adjustments)
            var adjustedGrossIncome = grossIncome;

            // Determine deduction amount
            var standardDeduction = _standardDeductions[taxReturn.FilingStatus];
            var deductionAmount = Math.Max(standardDeduction, totalDeductions);

            // Calculate taxable income
            var taxableIncome = Math.Max(0, adjustedGrossIncome - deductionAmount);

            // Calculate tax using brackets
            var brackets = _taxBrackets[taxReturn.FilingStatus];
            var totalTax = CalculateTaxFromBrackets(taxableIncome, brackets);

            // Calculate effective and marginal rates
            var effectiveTaxRate = grossIncome > 0 ? (totalTax / grossIncome) * 100 : 0;
            var marginalTaxRate = GetMarginalTaxRate(taxableIncome, brackets);

            return new TaxCalculation(
                grossIncome,
                adjustedGrossIncome,
                taxableIncome,
                totalTax,
                effectiveTaxRate,
                marginalTaxRate,
                Clock.Now
            );
        }

        private decimal CalculateTaxFromBrackets(
            decimal taxableIncome,
            List<TaxBracket> brackets)
        {
            var bracket = brackets.FirstOrDefault(b => b.IsInBracket(taxableIncome));
            return bracket?.CalculateTax(taxableIncome) ?? 0;
        }

        private decimal GetMarginalTaxRate(
            decimal taxableIncome,
            List<TaxBracket> brackets)
        {
            var bracket = brackets.FirstOrDefault(b => b.IsInBracket(taxableIncome));
            return bracket?.TaxRate ?? 0;
        }

        public decimal GetStandardDeduction(TaxFilingStatus filingStatus)
        {
            return _standardDeductions[filingStatus];
        }

        public decimal EstimateQuarterlyTaxPayment(
            decimal estimatedAnnualIncome,
            TaxFilingStatus filingStatus)
        {
            // Simplified quarterly estimate
            var brackets = _taxBrackets[filingStatus];
            var estimatedTax = CalculateTaxFromBrackets(estimatedAnnualIncome, brackets);
            return estimatedTax / 4;
        }
    }
}
