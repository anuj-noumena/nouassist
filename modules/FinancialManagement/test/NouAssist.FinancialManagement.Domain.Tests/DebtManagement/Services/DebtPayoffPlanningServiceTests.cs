using System;
using System.Collections.Generic;
using NouAssist.DebtManagement.Aggregates;
using NouAssist.DebtManagement.Enums;
using NouAssist.DebtManagement.Services;
using Shouldly;
using Volo.Abp.Guids;
using Xunit;

namespace NouAssist.DebtManagement.Tests.Services
{
    public class DebtPayoffPlanningServiceTests
    {
        private readonly DebtPayoffPlanningService _service;
        private readonly IGuidGenerator _guidGenerator;

        public DebtPayoffPlanningServiceTests()
        {
            _guidGenerator = new SequentialGuidGenerator();
            _service = new DebtPayoffPlanningService { GuidGenerator = _guidGenerator };
        }

        [Fact]
        public void CreateOptimizedPayoffPlan_HighestInterestFirst_ShouldPrioritizeCorrectly()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var debts = CreateTestDebts(userId);
            var monthlyBudget = 1000m;

            // Act
            var plan = _service.CreateOptimizedPayoffPlan(
                userId,
                null,
                "Avalanche Plan",
                PaymentStrategy.HighestInterestFirst,
                monthlyBudget,
                debts
            );

            // Assert
            plan.ShouldNotBeNull();
            plan.Strategy.ShouldBe(PaymentStrategy.HighestInterestFirst);
            plan.Debts.Count.ShouldBe(3);
            
            // Verify highest interest debt gets priority (extra payment)
            var firstDebt = plan.Debts.ShouldContain(d => d.PayoffOrder == 1);
            firstDebt.AllocatedMonthlyPayment.ShouldBeGreaterThan(debts[0].MinimumPayment);
        }

        [Fact]
        public void CreateOptimizedPayoffPlan_LowestBalanceFirst_ShouldPrioritizeCorrectly()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var debts = CreateTestDebts(userId);
            var monthlyBudget = 1000m;

            // Act
            var plan = _service.CreateOptimizedPayoffPlan(
                userId,
                null,
                "Snowball Plan",
                PaymentStrategy.LowestBalanceFirst,
                monthlyBudget,
                debts
            );

            // Assert
            plan.Strategy.ShouldBe(PaymentStrategy.LowestBalanceFirst);
            var firstDebt = plan.Debts.ShouldContain(d => d.PayoffOrder == 1);
            firstDebt.AllocatedMonthlyPayment.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void CreateOptimizedPayoffPlan_InsufficientBudget_ShouldThrowException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var debts = CreateTestDebts(userId);
            var insufficientBudget = 100m; // Less than sum of minimums

            // Act & Assert
            Should.Throw<Volo.Abp.BusinessException>(() =>
                _service.CreateOptimizedPayoffPlan(
                    userId,
                    null,
                    "Plan",
                    PaymentStrategy.HighestInterestFirst,
                    insufficientBudget,
                    debts
                )
            );
        }

        [Fact]
        public void AnalyzePayoffPlan_ShouldCalculateCorrectTotals()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var debts = CreateTestDebts(userId);
            var plan = _service.CreateOptimizedPayoffPlan(
                userId,
                null,
                "Test Plan",
                PaymentStrategy.HighestInterestFirst,
                1000m,
                debts
            );

            // Act
            var analysis = _service.AnalyzePayoffPlan(plan, debts);

            // Assert
            analysis.ShouldNotBeNull();
            analysis.TotalDebt.ShouldBe(debts[0].CurrentBalance + debts[1].CurrentBalance + debts[2].CurrentBalance);
            analysis.MonthlyPayment.ShouldBe(1000m);
            analysis.MonthsToPayoff.ShouldBeGreaterThan(0);
        }

        private List<DebtAccount> CreateTestDebts(Guid userId)
        {
            return new List<DebtAccount>
            {
                new DebtAccount(
                    Guid.NewGuid(),
                    userId,
                    null,
                    "Credit Card",
                    DebtType.CreditCard,
                    5000m,
                    5000m,
                    18.5m,
                    150m,
                    PaymentFrequency.Monthly
                ),
                new DebtAccount(
                    Guid.NewGuid(),
                    userId,
                    null,
                    "Personal Loan",
                    DebtType.PersonalLoan,
                    10000m,
                    8000m,
                    12.0m,
                    250m,
                    PaymentFrequency.Monthly
                ),
                new DebtAccount(
                    Guid.NewGuid(),
                    userId,
                    null,
                    "Auto Loan",
                    DebtType.AutoLoan,
                    15000m,
                    12000m,
                    5.5m,
                    300m,
                    PaymentFrequency.Monthly
                )
            };
        }
    }
}
