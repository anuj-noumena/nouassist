using System;
using NouAssist.TaxManagement.Aggregates;
using NouAssist.TaxManagement.Enums;
using NouAssist.TaxManagement.Services;
using Shouldly;
using Xunit;

namespace NouAssist.TaxManagement.Tests.Services
{
    public class TaxCalculationServiceTests
    {
        private readonly TaxCalculationService _service;

        public TaxCalculationServiceTests()
        {
            _service = new TaxCalculationService();
        }

        [Theory]
        [InlineData(50000, TaxFilingStatus.Single, 6307)] // Approximate expected tax
        [InlineData(100000, TaxFilingStatus.Single, 17400)] // Approximate
        [InlineData(100000, TaxFilingStatus.MarriedFilingJointly, 13100)] // Approximate
        public void CalculateTaxLiability_ShouldCalculateCorrectTax(
            decimal grossIncome,
            TaxFilingStatus filingStatus,
            decimal expectedTaxApproximate)
        {
            // Arrange
            var taxReturn = CreateTaxReturn(filingStatus, grossIncome);

            // Act
            var calculation = _service.CalculateTaxLiability(taxReturn, 0);

            // Assert
            calculation.GrossIncome.ShouldBe(grossIncome);
            calculation.TotalTaxLiability.ShouldBeInRange(
                expectedTaxApproximate - 500, 
                expectedTaxApproximate + 500
            ); // Allow some variance
            calculation.EffectiveTaxRate.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void GetStandardDeduction_ShouldReturnCorrectAmount()
        {
            // Act
            var singleDeduction = _service.GetStandardDeduction(TaxFilingStatus.Single);
            var jointDeduction = _service.GetStandardDeduction(TaxFilingStatus.MarriedFilingJointly);

            // Assert
            singleDeduction.ShouldBe(13850m);
            jointDeduction.ShouldBe(27700m);
        }

        [Fact]
        public void EstimateQuarterlyTaxPayment_ShouldCalculateCorrectAmount()
        {
            // Arrange
            var estimatedAnnualIncome = 80000m;

            // Act
            var quarterlyPayment = _service.EstimateQuarterlyTaxPayment(
                estimatedAnnualIncome,
                TaxFilingStatus.Single
            );

            // Assert
            quarterlyPayment.ShouldBeGreaterThan(0);
            quarterlyPayment.ShouldBeLessThan(estimatedAnnualIncome / 4); // Should be less than 25% of income
        }

        private TaxReturn CreateTaxReturn(TaxFilingStatus filingStatus, decimal income)
        {
            var taxReturn = new TaxReturn(
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                2024,
                filingStatus
            );

            taxReturn.UpdateIncome(income, 0, 0, 0);
            return taxReturn;
        }
    }
}
