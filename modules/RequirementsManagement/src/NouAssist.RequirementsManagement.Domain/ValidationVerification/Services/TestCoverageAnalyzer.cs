using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using NouAssist.RequirementsManagement.ValidationVerification.Repositories;

namespace NouAssist.RequirementsManagement.ValidationVerification.Services
{
    /// <summary>
    /// Domain service for analyzing test coverage for requirements and generating coverage reports
    /// </summary>
    public class TestCoverageAnalyzer : DomainService, ITestCoverageAnalyzer
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly ILogger<TestCoverageAnalyzer> _logger;

        public TestCoverageAnalyzer(
            ITestCaseRepository testCaseRepository,
            ILogger<TestCoverageAnalyzer> logger)
        {
            _testCaseRepository = testCaseRepository;
            _logger = logger;
        }

        /// <summary>
        /// Analyzes test coverage for a specific requirement
        /// </summary>
        public async Task<RequirementTestCoverage> AnalyzeRequirementCoverageAsync(Guid requirementId)
        {
            _logger.LogInformation("Analyzing test coverage for requirement {RequirementId}", requirementId);

            try
            {
                // Get all test cases linked to the requirement
                var testCases = await _testCaseRepository.GetTestCasesForRequirementAsync(requirementId);

                var coverage = new RequirementTestCoverage
                {
                    RequirementId = requirementId,
                    TotalTestCases = testCases.Count,
                    TestCasesByStatus = testCases.GroupBy(tc => tc.Status)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    TestCasesByPriority = testCases.GroupBy(tc => tc.Priority)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    ExecutionStatistics = await CalculateExecutionStatisticsAsync(testCases),
                    CoverageScore = CalculateCoverageScore(testCases),
                    LastExecuted = testCases.SelectMany(tc => tc.ExecutionLogs)
                        .Where(el => el.IsCompleted())
                        .OrderByDescending(el => el.CompletedAt)
                        .FirstOrDefault()?.CompletedAt
                };

                _logger.LogInformation("Completed test coverage analysis for requirement {RequirementId}. " +
                    "Total test cases: {Count}, Coverage score: {Score}", 
                    requirementId, coverage.TotalTestCases, coverage.CoverageScore);

                return coverage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to analyze test coverage for requirement {RequirementId}", requirementId);
                throw;
            }
        }

        /// <summary>
        /// Analyzes test coverage for multiple requirements
        /// </summary>
        public async Task<List<RequirementTestCoverage>> AnalyzeMultipleRequirementsCoverageAsync(
            List<Guid> requirementIds)
        {
            _logger.LogInformation("Analyzing test coverage for {Count} requirements", requirementIds.Count);

            var coverageList = new List<RequirementTestCoverage>();

            foreach (var requirementId in requirementIds)
            {
                try
                {
                    var coverage = await AnalyzeRequirementCoverageAsync(requirementId);
                    coverageList.Add(coverage);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to analyze coverage for requirement {RequirementId}", requirementId);
                    
                    // Add a placeholder entry with error information
                    coverageList.Add(new RequirementTestCoverage
                    {
                        RequirementId = requirementId,
                        TotalTestCases = 0,
                        CoverageScore = 0,
                        HasErrors = true,
                        ErrorMessage = ex.Message
                    });
                }
            }

            return coverageList;
        }

        /// <summary>
        /// Generates a comprehensive test coverage report
        /// </summary>
        public async Task<TestCoverageReport> GenerateCoverageReportAsync(
            List<Guid> requirementIds,
            DateTime reportDate,
            string reportScope = null)
        {
            _logger.LogInformation("Generating test coverage report for {Count} requirements", requirementIds.Count);

            try
            {
                var requirementCoverages = await AnalyzeMultipleRequirementsCoverageAsync(requirementIds);

                var report = new TestCoverageReport
                {
                    ReportId = GuidGenerator.Create(),
                    GeneratedAt = reportDate,
                    Scope = reportScope ?? "Requirements Coverage Analysis",
                    TotalRequirements = requirementIds.Count,
                    RequirementCoverages = requirementCoverages,
                    Summary = CalculateReportSummary(requirementCoverages),
                    Recommendations = GenerateRecommendations(requirementCoverages)
                };

                _logger.LogInformation("Generated test coverage report {ReportId} with overall score: {Score}", 
                    report.ReportId, report.Summary.OverallCoverageScore);

                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate test coverage report");
                throw;
            }
        }

        /// <summary>
        /// Identifies requirements with insufficient test coverage
        /// </summary>
        public async Task<List<RequirementTestGap>> IdentifyTestGapsAsync(
            List<Guid> requirementIds,
            double minimumCoverageThreshold = 0.8)
        {
            _logger.LogInformation("Identifying test gaps for {Count} requirements with threshold {Threshold}", 
                requirementIds.Count, minimumCoverageThreshold);

            var coverages = await AnalyzeMultipleRequirementsCoverageAsync(requirementIds);
            var testGaps = new List<RequirementTestGap>();

            foreach (var coverage in coverages)
            {
                if (coverage.CoverageScore < minimumCoverageThreshold)
                {
                    var gap = new RequirementTestGap
                    {
                        RequirementId = coverage.RequirementId,
                        CurrentCoverageScore = coverage.CoverageScore,
                        TargetCoverageScore = minimumCoverageThreshold,
                        GapSeverity = CalculateGapSeverity(coverage.CoverageScore, minimumCoverageThreshold),
                        RecommendedActions = GenerateGapRecommendations(coverage)
                    };

                    testGaps.Add(gap);
                }
            }

            _logger.LogInformation("Identified {Count} requirements with test coverage gaps", testGaps.Count);
            return testGaps;
        }

        private async Task<TestExecutionStatistics> CalculateExecutionStatisticsAsync(List<TestCase> testCases)
        {
            var allExecutions = testCases.SelectMany(tc => tc.ExecutionLogs).ToList();
            var completedExecutions = allExecutions.Where(el => el.IsCompleted()).ToList();

            return new TestExecutionStatistics
            {
                TotalExecutions = completedExecutions.Count,
                PassedExecutions = completedExecutions.Count(el => el.Status == TestExecutionStatus.Pass),
                FailedExecutions = completedExecutions.Count(el => el.Status == TestExecutionStatus.Fail),
                BlockedExecutions = completedExecutions.Count(el => el.Status == TestExecutionStatus.Blocked),
                LastExecutionDate = completedExecutions.OrderByDescending(el => el.CompletedAt)
                    .FirstOrDefault()?.CompletedAt,
                AverageExecutionTime = completedExecutions.Any() 
                    ? TimeSpan.FromTicks((long)completedExecutions.Average(el => el.ExecutionDuration?.Ticks ?? 0))
                    : (TimeSpan?)null
            };
        }

        private double CalculateCoverageScore(List<TestCase> testCases)
        {
            if (!testCases.Any())
                return 0.0;

            // Basic coverage calculation based on test case status and execution results
            double score = 0.0;
            
            foreach (var testCase in testCases)
            {
                switch (testCase.Status)
                {
                    case TestCaseStatus.Ready:
                        score += 0.7; // Ready but not executed
                        break;
                    case TestCaseStatus.InExecution:
                        score += 0.8; // Currently being executed
                        break;
                    case TestCaseStatus.Draft:
                        score += 0.3; // Draft state
                        break;
                    default:
                        break;
                }

                // Additional score for successful executions
                var latestExecution = testCase.ExecutionLogs
                    .Where(el => el.IsCompleted())
                    .OrderByDescending(el => el.CompletedAt)
                    .FirstOrDefault();

                if (latestExecution?.Status == TestExecutionStatus.Pass)
                {
                    score += 0.3; // Bonus for passed execution
                }
            }

            return Math.Min(1.0, score / testCases.Count);
        }

        private TestCoverageSummary CalculateReportSummary(List<RequirementTestCoverage> coverages)
        {
            var validCoverages = coverages.Where(c => !c.HasErrors).ToList();

            return new TestCoverageSummary
            {
                OverallCoverageScore = validCoverages.Any() 
                    ? validCoverages.Average(c => c.CoverageScore) 
                    : 0.0,
                TotalTestCases = validCoverages.Sum(c => c.TotalTestCases),
                RequirementsWithFullCoverage = validCoverages.Count(c => c.CoverageScore >= 0.9),
                RequirementsWithNoCoverage = validCoverages.Count(c => c.TotalTestCases == 0),
                RequirementsWithErrors = coverages.Count(c => c.HasErrors),
                HighestCoverageScore = validCoverages.Any() 
                    ? validCoverages.Max(c => c.CoverageScore) 
                    : 0.0,
                LowestCoverageScore = validCoverages.Any() 
                    ? validCoverages.Min(c => c.CoverageScore) 
                    : 0.0
            };
        }

        private List<string> GenerateRecommendations(List<RequirementTestCoverage> coverages)
        {
            var recommendations = new List<string>();

            var validCoverages = coverages.Where(c => !c.HasErrors).ToList();
            var noCoverageCount = validCoverages.Count(c => c.TotalTestCases == 0);
            var lowCoverageCount = validCoverages.Count(c => c.CoverageScore < 0.5 && c.TotalTestCases > 0);

            if (noCoverageCount > 0)
            {
                recommendations.Add($"Create test cases for {noCoverageCount} requirements with no test coverage");
            }

            if (lowCoverageCount > 0)
            {
                recommendations.Add($"Improve test coverage for {lowCoverageCount} requirements with low coverage scores");
            }

            var failedTestsCount = validCoverages.Sum(c => 
                c.ExecutionStatistics?.FailedExecutions ?? 0);
            
            if (failedTestsCount > 0)
            {
                recommendations.Add($"Investigate and fix {failedTestsCount} failed test executions");
            }

            var lastExecuted = validCoverages
                .Where(c => c.LastExecuted.HasValue)
                .OrderByDescending(c => c.LastExecuted.Value)
                .FirstOrDefault()?.LastExecuted;

            if (lastExecuted.HasValue && lastExecuted.Value < DateTime.UtcNow.AddDays(-30))
            {
                recommendations.Add("Consider running test executions as some requirements haven't been tested recently");
            }

            return recommendations;
        }

        private TestGapSeverity CalculateGapSeverity(double currentScore, double targetScore)
        {
            var gap = targetScore - currentScore;
            
            return gap switch
            {
                >= 0.5 => TestGapSeverity.Critical,
                >= 0.3 => TestGapSeverity.High,
                >= 0.1 => TestGapSeverity.Medium,
                _ => TestGapSeverity.Low
            };
        }

        private List<string> GenerateGapRecommendations(RequirementTestCoverage coverage)
        {
            var recommendations = new List<string>();

            if (coverage.TotalTestCases == 0)
            {
                recommendations.Add("Create initial test cases for this requirement");
                recommendations.Add("Define test scenarios covering different aspects of the requirement");
            }
            else
            {
                if (coverage.ExecutionStatistics?.TotalExecutions == 0)
                {
                    recommendations.Add("Execute existing test cases to validate the requirement");
                }

                if (coverage.ExecutionStatistics?.FailedExecutions > 0)
                {
                    recommendations.Add("Investigate and resolve failed test cases");
                }

                if (coverage.CoverageScore < 0.7)
                {
                    recommendations.Add("Add additional test cases to improve coverage");
                    recommendations.Add("Consider edge cases and negative scenarios");
                }

                var draftTestCases = coverage.TestCasesByStatus.GetValueOrDefault(TestCaseStatus.Draft, 0);
                if (draftTestCases > 0)
                {
                    recommendations.Add($"Complete {draftTestCases} draft test cases and make them ready for execution");
                }
            }

            return recommendations;
        }
    }

    // Supporting data structures for test coverage analysis

    /// <summary>
    /// Test coverage information for a specific requirement
    /// </summary>
    public class RequirementTestCoverage
    {
        public Guid RequirementId { get; set; }
        public int TotalTestCases { get; set; }
        public Dictionary<TestCaseStatus, int> TestCasesByStatus { get; set; } = new();
        public Dictionary<TestCasePriority, int> TestCasesByPriority { get; set; } = new();
        public TestExecutionStatistics ExecutionStatistics { get; set; }
        public double CoverageScore { get; set; }
        public DateTime? LastExecuted { get; set; }
        public bool HasErrors { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Test execution statistics aggregation
    /// </summary>
    public class TestExecutionStatistics
    {
        public int TotalExecutions { get; set; }
        public int PassedExecutions { get; set; }
        public int FailedExecutions { get; set; }
        public int BlockedExecutions { get; set; }
        public DateTime? LastExecutionDate { get; set; }
        public TimeSpan? AverageExecutionTime { get; set; }
    }

    /// <summary>
    /// Comprehensive test coverage report
    /// </summary>
    public class TestCoverageReport
    {
        public Guid ReportId { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string Scope { get; set; }
        public int TotalRequirements { get; set; }
        public List<RequirementTestCoverage> RequirementCoverages { get; set; } = new();
        public TestCoverageSummary Summary { get; set; }
        public List<string> Recommendations { get; set; } = new();
    }

    /// <summary>
    /// Summary statistics for test coverage report
    /// </summary>
    public class TestCoverageSummary
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
    /// Represents a test coverage gap for a requirement
    /// </summary>
    public class RequirementTestGap
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
