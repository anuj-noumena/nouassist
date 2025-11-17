using System.Collections.Generic;
using NouAssist.FinancialManagement.TaxManagement.ValueObjects;
using NouAssist.TaxManagement.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.TaxManagement.Services
{
    public class DeductionValidationService : DomainService
    {
        // Deduction limits for 2024 (example - should be configurable)
        private readonly Dictionary<DeductionCategory, DeductionLimit> _deductionLimits = new()
        {
            {
                DeductionCategory.CharitableContributions,
                new DeductionLimit(
                    maxAmount: decimal.MaxValue,
                    percentageOfIncome: 0.60m, // 60% of AGI
                    hasPhaseOut: false
                )
            },
            {
                DeductionCategory.StateTaxes,
                new DeductionLimit(
                    maxAmount: 10000m, // SALT cap
                    percentageOfIncome: 1.0m,
                    hasPhaseOut: false
                )
            },
            {
                DeductionCategory.MortgageInterest,
                new DeductionLimit(
                    maxAmount: 750000m, // Loan amount limit
                    percentageOfIncome: 1.0m,
                    hasPhaseOut: false
                )
            },
            {
                DeductionCategory.MedicalExpenses,
                new DeductionLimit(
                    maxAmount: decimal.MaxValue,
                    percentageOfIncome: 0.075m, // Only above 7.5% of AGI
                    hasPhaseOut: false
                )
            },
            {
                DeductionCategory.StudentLoanInterest,
                new DeductionLimit(
                    maxAmount: 2500m,
                    percentageOfIncome: 1.0m,
                    hasPhaseOut: true,
                    phaseOutStartIncome: 75000m,
                    phaseOutRate: 0.05m
                )
            }
        };

        public DeductionValidationResult ValidateDeduction(
            DeductionCategory category,
            decimal claimedAmount,
            decimal adjustedGrossIncome)
        {
            if (!_deductionLimits.ContainsKey(category))
            {
                // No specific limit - allow full amount
                return new DeductionValidationResult(
                    true,
                    claimedAmount,
                    claimedAmount,
                    null
                );
            }

            var limit = _deductionLimits[category];
            var allowedAmount = limit.CalculateAllowedDeduction(claimedAmount, adjustedGrossIncome);

            var isValid = allowedAmount >= claimedAmount;
            var message = isValid
                ? null
                : $"Deduction limited to {allowedAmount:C} based on income and IRS rules";

            return new DeductionValidationResult(
                isValid,
                claimedAmount,
                allowedAmount,
                message
            );
        }

        public bool IsDeductible(
            DeductionCategory category,
            string description,
            decimal amount)
        {
            // Basic deductibility check
            if (amount <= 0) return false;

            // Category-specific checks
            return category switch
            {
                DeductionCategory.BusinessExpenses => true,
                DeductionCategory.CharitableContributions => amount >= 0,
                DeductionCategory.MedicalExpenses => true,
                _ => true
            };
        }
    }

    public class DeductionValidationResult
    {
        public bool IsValid { get; }
        public decimal ClaimedAmount { get; }
        public decimal AllowedAmount { get; }
        public string? ValidationMessage { get; }

        public DeductionValidationResult(
            bool isValid,
            decimal claimedAmount,
            decimal allowedAmount,
            string? validationMessage)
        {
            IsValid = isValid;
            ClaimedAmount = claimedAmount;
            AllowedAmount = allowedAmount;
            ValidationMessage = validationMessage;
        }
    }
}
