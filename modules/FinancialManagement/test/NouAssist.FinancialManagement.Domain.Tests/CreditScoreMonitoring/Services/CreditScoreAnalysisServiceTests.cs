using System;
using System.Collections.Generic;
using NouAssist.CreditScoreMonitoring.Aggregates;
using NouAssist.CreditScoreMonitoring.Enums;
using NouAssist.CreditScoreMonitoring.Services;
using Shouldly;
using Xunit;

namespace NouAssist.CreditScoreMonitoring.Tests.Services
{
    public class CreditScoreAnalysisServiceTests
    {
        private readonly CreditScoreAnalysisService _service;

        public CreditScoreAnalysisServiceTests()
        {
            _service = new CreditScoreAnalysisService();
        }

        [Fact]
        public void AnalyzeCreditHealth_WithGoodCredit_ShouldReturnPositiveAnalysis()
        {
            // Arrange
            var report = CreateHealthyCreditReport();

            // Act
            var healthReport = _service.AnalyzeCreditHealth(report);

            // Assert
            healthReport.ShouldNotBeNull();
            healthReport.OverallHealthScore.ShouldBeGreaterThan(70);
            healthReport.HealthLevel.ShouldBeOneOf("Good", "Excellent");
            healthReport.Factors.ShouldNotBeEmpty();
        }

        [Fact]
        public void AnalyzeCreditHealth_WithHighUtilization_ShouldFlagAsNegative()
        {
            // Arrange
            var report = CreateCreditReportWithHighUtilization();

            // Act
            var healthReport = _service.AnalyzeCreditHealth(report);

            // Assert
            var utilizationFactor = healthReport.Factors
                .ShouldContain(f => f.Name == "Credit Utilization");
            utilizationFactor.Impact.ShouldBe("Negative");
            healthReport.Recommendations.ShouldContain(r => r.Contains("utilization"));
        }

        [Fact]
        public void ProjectScoreImprovement_WithUtilizationReduction_ShouldProjectGain()
        {
            // Arrange
            var report = CreateCreditReportWithHighUtilization();
            var plan = new CreditImprovementPlan
            {
                ReduceUtilizationTo = 20m,
                PayOffDelinquentAccounts = false,
                MonthsToWait = 0
            };

            // Act
            var projection = _service.ProjectScoreImprovement(report, plan);

            // Assert
            projection.ProjectedGain.ShouldBeGreaterThan(0);
            projection.ProjectedScore.ShouldBeGreaterThan(projection.CurrentScore);
            projection.TimelineMonths.ShouldBeGreaterThan(0);
        }

        private CreditReport CreateHealthyCreditReport()
        {
            var report = new CreditReport(
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                CreditBureau.Experian,
                DateTime.UtcNow,
                750
            );

            report.AddAccount(
                "ACC001",
                "Bank of America",
                "Credit Card",
                AccountStatus.Open,
                10000m,
                2000m, // 20% utilization
                DateTime.UtcNow.AddYears(-5)
            );

            return report;
        }

        private CreditReport CreateCreditReportWithHighUtilization()
        {
            var report = new CreditReport(
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                CreditBureau.Experian,
                DateTime.UtcNow,
                650
            );

            report.AddAccount(
                "ACC001",
                "Bank of America",
                "Credit Card",
                AccountStatus.Open,
                10000m,
                7000m, // 70% utilization
                DateTime.UtcNow.AddYears(-2)
            );

            return report;
        }
    }
}
