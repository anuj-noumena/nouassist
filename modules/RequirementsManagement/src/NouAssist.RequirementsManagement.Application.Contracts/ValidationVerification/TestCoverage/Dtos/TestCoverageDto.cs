using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCoverage.Dtos
{
    /// <summary>
    /// DTO for requirement test coverage information
    /// </summary>
    public class RequirementTestCoverageDto
    {
        public Guid RequirementId { get; set; }
        public int TotalTestCases { get; set; }
        public Dictionary<TestCaseStatus, int> TestCasesByStatus { get; set; } = new();
        public Dictionary<TestCasePriority, int> TestCasesByPriority { get; set; } = new();
        public TestExecutionStatisticsDto ExecutionStatistics { get; set; }
        public double CoverageScore { get; set; }
        public DateTime? LastExecuted { get; set; }
        public bool HasErrors { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// DTO for test execution statistics
    /// </summary>
    public class TestExecutionStatisticsDto
    {
        public int TotalExecutions { get; set; }
        public int PassedExecutions { get; set; }
        public int FailedExecutions { get; set; }
        public int BlockedExecutions { get; set; }
        public DateTime? LastExecutionDate { get; set; }
        public TimeSpan? AverageExecutionTime { get; set; }
    }

    /// <summary>
    /// DTO for comprehensive test coverage report
    /// </summary>
    public class TestCoverageReportDto
    {
        public Guid ReportId { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string Scope { get; set; }
        public int TotalRequirements { get; set; }
        public List<RequirementTestCoverageDto> RequirementCoverages { get; set; } = new();
        public TestCoverageSummaryDto Summary { get; set; }
        public List<string> Recommendations { get; set; } = new();
    }

    /// <summary>
    /// DTO for test coverage summary
    /// </summary>
    public class TestCoverageSummaryDto
    {
        public double OverallCoverageScore { get; set; }
        public int TotalTestCases { get; set; }
        public int RequirementsWithFullCoverage { get; set; }
        public int RequirementsWithNoCoverage { get; set; }
        public int RequirementsWithErrors { get; set; }
        public double HighestCoverageScore { get; set; }
        public double LowestCoverageScore { get; set; }
    }

    /// <summary>
    /// DTO for test coverage gap
    /// </summary>
    public class RequirementTestGapDto
    {
        public Guid RequirementId { get; set; }
        public double CurrentCoverageScore { get; set; }
        public double TargetCoverageScore { get; set; }
        public TestGapSeverity GapSeverity { get; set; }
        public List<string> RecommendedActions { get; set; } = new();
    }

    /// <summary>
    /// Severity levels for test coverage gaps
    /// </summary>
    public enum TestGapSeverity
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 3
    }
}
