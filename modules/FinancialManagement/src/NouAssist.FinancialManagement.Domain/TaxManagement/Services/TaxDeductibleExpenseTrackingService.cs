using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.TaxManagement.Services
{
    /// <summary>
    /// Domain service for tracking tax-deductible expenses
    /// </summary>
    public class TaxDeductibleExpenseTrackingService : DomainService
    {
        private readonly ILogger<TaxDeductibleExpenseTrackingService> _logger;

        public TaxDeductibleExpenseTrackingService(
            ILogger<TaxDeductibleExpenseTrackingService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Categorize expense for tax purposes
        /// </summary>
        public TaxExpenseCategory CategorizeExpenseForTax(
            string categoryName,
            decimal amount,
            string description)
        {
            // Business expenses
            if (IsBusinessExpense(categoryName, description))
            {
                return CategorizeBusinessExpense(categoryName);
            }

            // Medical expenses
            if (IsMedicalExpense(categoryName))
            {
                return new TaxExpenseCategory
                {
                    TaxCategoryCode = "Medical",
                    TaxCategoryName = "Medical and Dental Expenses",
                    DeductibilityPercentage = 100,
                    HasLimit = true,
                    LimitDescription = "Only amount exceeding 7.5% of AGI",
                    FormSchedule = "Schedule A",
                    IsItemizedDeduction = true
                };
            }

            // Charitable contributions
            if (IsCharitableContribution(categoryName))
            {
                return new TaxExpenseCategory
                {
                    TaxCategoryCode = "Charitable",
                    TaxCategoryName = "Charitable Contributions",
                    DeductibilityPercentage = 100,
                    HasLimit = true,
                    LimitDescription = "Up to 60% of AGI for cash donations",
                    FormSchedule = "Schedule A",
                    IsItemizedDeduction = true
                };
            }

            // State and local taxes
            if (IsStateLocalTax(categoryName))
            {
                return new TaxExpenseCategory
                {
                    TaxCategoryCode = "SALT",
                    TaxCategoryName = "State and Local Taxes",
                    DeductibilityPercentage = 100,
                    HasLimit = true,
                    LimitDescription = "Limited to $10,000",
                    FormSchedule = "Schedule A",
                    IsItemizedDeduction = true
                };
            }

            // Mortgage interest
            if (IsMortgageInterest(categoryName))
            {
                return new TaxExpenseCategory
                {
                    TaxCategoryCode = "MortgageInterest",
                    TaxCategoryName = "Home Mortgage Interest",
                    DeductibilityPercentage = 100,
                    HasLimit = true,
                    LimitDescription = "On up to $750,000 of debt",
                    FormSchedule = "Schedule A",
                    IsItemizedDeduction = true
                };
            }

            // Not deductible
            return new TaxExpenseCategory
            {
                TaxCategoryCode = "NonDeductible",
                TaxCategoryName = "Not Tax Deductible",
                DeductibilityPercentage = 0,
                HasLimit = false,
                FormSchedule = null,
                IsItemizedDeduction = false
            };
        }

        private TaxExpenseCategory CategorizeBusinessExpense(string categoryName)
        {
            // Meals and entertainment - 50% deductible
            if (categoryName.Contains("Meal", StringComparison.OrdinalIgnoreCase) ||
                categoryName.Contains("Restaurant", StringComparison.OrdinalIgnoreCase))
            {
                return new TaxExpenseCategory
                {
                    TaxCategoryCode = "BusinessMeals",
                    TaxCategoryName = "Business Meals",
                    DeductibilityPercentage = 50,
                    HasLimit = false,
                    FormSchedule = "Schedule C",
                    IsItemizedDeduction = false
                };
            }

            // Fully deductible business expenses
            return new TaxExpenseCategory
            {
                TaxCategoryCode = "BusinessExpense",
                TaxCategoryName = DetermineBusinessCategory(categoryName),
                DeductibilityPercentage = 100,
                HasLimit = false,
                FormSchedule = "Schedule C",
                IsItemizedDeduction = false
            };
        }

        private string DetermineBusinessCategory(string categoryName)
        {
            if (categoryName.Contains("Travel", StringComparison.OrdinalIgnoreCase))
            {
                return "Travel Expenses";
            }

            if (categoryName.Contains("Office", StringComparison.OrdinalIgnoreCase))
            {
                return "Office Expenses";
            }

            if (categoryName.Contains("Supplies", StringComparison.OrdinalIgnoreCase))
            {
                return "Supplies";
            }

            if (categoryName.Contains("Advertising", StringComparison.OrdinalIgnoreCase))
            {
                return "Advertising";
            }

            return "Other Business Expenses";
        }

        private bool IsBusinessExpense(string categoryName, string description)
        {
            var businessKeywords = new[] { "business", "office", "supplies", "travel", "advertising", "professional" };
            return businessKeywords.Any(k =>
                categoryName.Contains(k, StringComparison.OrdinalIgnoreCase) ||
                description.Contains(k, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsMedicalExpense(string categoryName)
        {
            var medicalKeywords = new[] { "medical", "dental", "doctor", "hospital", "prescription", "healthcare" };
            return medicalKeywords.Any(k => categoryName.Contains(k, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsCharitableContribution(string categoryName)
        {
            var charityKeywords = new[] { "charity", "donation", "nonprofit", "contribution" };
            return charityKeywords.Any(k => categoryName.Contains(k, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsStateLocalTax(string categoryName)
        {
            var taxKeywords = new[] { "property tax", "state tax", "local tax", "real estate tax" };
            return taxKeywords.Any(k => categoryName.Contains(k, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsMortgageInterest(string categoryName)
        {
            return categoryName.Contains("mortgage", StringComparison.OrdinalIgnoreCase) &&
                   categoryName.Contains("interest", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Calculate total deductible amount for year-to-date
        /// </summary>
        public async Task<TaxDeductionSummary> CalculateYearToDateDeductionsAsync(
            List<TaxDeductibleExpenseRecord> expenses,
            int taxYear)
        {
            var yearExpenses = expenses.Where(e => e.ExpenseDate.Year == taxYear).ToList();

            var businessTotal = yearExpenses
                .Where(e => e.TaxCategory?.FormSchedule == "Schedule C")
                .Sum(e => e.Amount * (e.TaxCategory.DeductibilityPercentage / 100m));

            var itemizedTotal = yearExpenses
                .Where(e => e.TaxCategory?.IsItemizedDeduction == true)
                .Sum(e => e.Amount * (e.TaxCategory.DeductibilityPercentage / 100m));

            return new TaxDeductionSummary
            {
                TaxYear = taxYear,
                TotalBusinessDeductions = businessTotal,
                TotalItemizedDeductions = itemizedTotal,
                TotalDeductions = businessTotal + itemizedTotal,
                ExpenseCount = yearExpenses.Count
            };
        }
    }

    public class TaxExpenseCategory
    {
        public string TaxCategoryCode { get; set; }
        public string TaxCategoryName { get; set; }
        public decimal DeductibilityPercentage { get; set; }
        public bool HasLimit { get; set; }
        public string LimitDescription { get; set; }
        public string FormSchedule { get; set; }
        public bool IsItemizedDeduction { get; set; }
    }

    public class TaxDeductibleExpenseRecord
    {
        public Guid ExpenseId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public TaxExpenseCategory TaxCategory { get; set; }
    }

    public class TaxDeductionSummary
    {
        public int TaxYear { get; set; }
        public decimal TotalBusinessDeductions { get; set; }
        public decimal TotalItemizedDeductions { get; set; }
        public decimal TotalDeductions { get; set; }
        public int ExpenseCount { get; set; }
    }
}
